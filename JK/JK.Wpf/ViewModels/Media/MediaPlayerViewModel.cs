using JK.Base.Logs;
using JK.Wpf.UserControls.Media;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace JK.Wpf.ViewModels.Media
{
    public class MediaPlayerViewModel : ActivatableViewModel
    {
        public MediaPlayerViewModel(Dispatcher dispatcher) : base(dispatcher)
        {

        }

        /// <summary>
        /// 영상 플레이 리스트
        /// </summary>
        public List<MediaPlayerModel> MediaPlayerList { get; set; } = new List<MediaPlayerModel>();
        /// <summary>
        /// 동작중인 영상 플레이
        /// </summary>
        public MediaPlayerModel CurrentPlayMedia { get; private set; } = null;
        /// <summary>
        /// 파일 다운로드 Paht
        /// </summary>
        public string DownloadPath { get; set; } = string.Empty;
        /// <summary>
        /// 이미지 표시 Interval(초)
        /// </summary>
        public int ImageInterval { get; set; } = 5;

        private string _VideoUri = null;
        /// <summary>
        /// 비디오 URI
        /// </summary>
        public string VideoUri
        {
            get { return _VideoUri; }
            set
            {
                _VideoUri = value;
                RaisePropertyChanged();
            }
        }

        private string _ImageUri = null;
        /// <summary>
        /// 이미지 URI
        /// </summary>
        public string ImageUri
        {
            get { return _ImageUri; }
            set
            {
                _ImageUri = value;
                RaisePropertyChanged();
            }
        }

        private MediaKindEnum _PlayMediaKind = MediaKindEnum.None;
        /// <summary>
        /// 현재 비디오 종류
        /// </summary>
        public MediaKindEnum PlayMediaKind
        {
            get { return _PlayMediaKind; }
            set
            {
                SetProperty(ref _PlayMediaKind, value);
            }
        }

        private bool _ListIsEmpty = false;
        /// <summary>
        /// 재생 리스트가 없음 여부
        /// </summary>
        public bool ListIsEmpty
        {
            get { return _ListIsEmpty; }
            set
            {
                SetProperty(ref _ListIsEmpty, value);
            }
        }

        public override Task LoadAsync()
        {
            return base.LoadAsync();
        }


        private bool IsPlay = false;
        /// <summary>
        /// 미디어 실행
        /// </summary>
        public void Play()
        {
            if (!IsPlay)
            {
                IsPlay = true;
            }
            else
            {
                return;
            }

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    try
                    {
                        await Task.Delay(500);

                        if (MediaPlayerList == null || MediaPlayerList.Count() == 0)
                        {
                            ListIsEmpty = true;
                            Log.i("MediaPlayerList Is Empty");
                            continue;
                        }

                        GetNextPlayMedia();

                        FileInfo fileInfo = new FileInfo(DownloadPath + @"\" + CurrentPlayMedia.Name.Trim());

                        if (!fileInfo.Exists)
                        {
                            RemoveCurrentPlayMedia();
                            continue;
                        }

                        if (CurrentPlayMedia.PalyDuration == new TimeSpan(0))
                        {
                            if (CurrentPlayMedia.MediaKind == MediaKindEnum.Video)
                            {
                                using (var shell = ShellObject.FromParsingName(fileInfo.FullName))
                                {
                                    IShellProperty prop = shell.Properties.System.Media.Duration;
                                    var t = (ulong)prop.ValueAsObject;
                                    CurrentPlayMedia.PalyDuration = TimeSpan.FromTicks((long)t);
                                }

                            }
                            else if (CurrentPlayMedia.MediaKind == MediaKindEnum.Image)
                            {
                                CurrentPlayMedia.PalyDuration = new TimeSpan(0, 0, ImageInterval);
                            }
                        }

                        PlayMediaKind = CurrentPlayMedia.MediaKind;

                        if (CurrentPlayMedia.MediaKind == MediaKindEnum.Video)
                        {
                            SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);

                            await Dispatcher.InvokeAsync(new Action(() =>
                            {
                                VideoUri = fileInfo.FullName;
                            }));
                        }
                        else if (CurrentPlayMedia.MediaKind == MediaKindEnum.Image)
                        {
                            try
                            {
                                await Dispatcher.InvokeAsync(new Action(() =>
                                {
                                    ImageUri = fileInfo.FullName;
                                }));

                            }
                            catch (Exception e)
                            {
                                Log.ei("MainViewModel NextPlay ImageUri Exception", e);
                                return;
                            }
                        }
                        else
                        {
                            RemoveCurrentPlayMedia();
                            continue;
                        }

                        Log.i($"MainViewModel Playe: {CurrentPlayMedia.MediaKind }, {CurrentPlayMedia.Name }");

                        await Task.Delay(CurrentPlayMedia.PalyDuration);
                    }
                    catch (Exception e)
                    {
                        Log.ei("MainViewModel NextPlay Exception", e);
                    }
                }
            });
        }

        /// <summary>
        /// 다음 미디어 설정
        /// </summary>
        /// <returns></returns>
        private void GetNextPlayMedia()
        {
            try
            {
                if (MediaPlayerList == null || MediaPlayerList.Count() == 0)
                {
                    return;
                }

                if (CurrentPlayMedia == null)
                {
                    CurrentPlayMedia = MediaPlayerList.First();
                }
                else
                {

                    using (var list = MediaPlayerList.GetEnumerator())
                    {
                        while (list.MoveNext())
                        {
                            if (CurrentPlayMedia.Name == list.Current.Name)
                            {
                                var haveNext = list.MoveNext();

                                if (haveNext)
                                {
                                    CurrentPlayMedia = list.Current;
                                }
                                else
                                {
                                    CurrentPlayMedia = MediaPlayerList.First();
                                }
                                
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.ei("WaitingManager Next Exception", e);
            }
        }

        /// <summary>
        /// 미디어 추가
        /// </summary>
        /// <param name="mediaPlayerModel"></param>
        public void AddPlayMedia(MediaPlayerModel mediaPlayerModel)
        {
            try
            {
                if (MediaPlayerList == null)
                {
                    MediaPlayerList = new List<MediaPlayerModel>();
                }

                MediaPlayerList.Add(mediaPlayerModel);
            }
            catch (Exception e)
            {
                Log.ei("WaitingManager AddPlayMedia Exception", e);
            }
        }

        /// <summary>
        /// 현재 미디어 삭제
        /// </summary>
        public void RemoveCurrentPlayMedia()
        {
            try
            {
                if (MediaPlayerList == null || MediaPlayerList.Count() == 0 || CurrentPlayMedia == null)
                {
                    return;
                }

                using (var list = MediaPlayerList.GetEnumerator())
                {
                    while (list.MoveNext())
                    {
                        if (CurrentPlayMedia.Name == list.Current.Name)
                        {
                            MediaPlayerList.Remove(list.Current);
                            Log.i($"WaitingManager CurrentRemove : {CurrentPlayMedia.Name}");

                            if (MediaPlayerList == null || MediaPlayerList.Count() == 0)
                            {
                                CurrentPlayMedia = null;
                            }
                            else
                            {
                                CurrentPlayMedia = MediaPlayerList.First();
                            }

                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.ei("WaitingManager CurrentRemove Exception", e);
            }
        }

        [DllImport("kernel32.dll")]
        private static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max);
    }

    public class MediaPlayerModel
    {
        public string Name { get; set; } = string.Empty;

        public MediaKindEnum MediaKind { get; set; } = MediaKindEnum.None;

        public TimeSpan PalyDuration { get; set; } = new TimeSpan(0);
    }
}