using Syadeu.Database;

namespace Syadeu
{
    public sealed class YOLOPresentationProvider : CLRSingleTone<YOLOPresentationProvider>
    {
        public YOLO_GameSystem GameSystem;
    }
}