namespace OpenGptChat.Common.Utilities
{
    public static class ColorHelper
    {
        public static double GetBrightness(int r, int g, int b)
        {
            // 将 RGB 值转换为 YUV 值
            double y = 0.299 * r + 0.587 * g + 0.114 * b;

            // 计算亮度值
            return y / 255;
        }
    }
}
