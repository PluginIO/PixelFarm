﻿//Apache2, 2014-2017, WinterDev

namespace LayoutFarm.UI
{
    public class UIPlatformWinNeutral : UIPlatform
    {
        PixelFarm.Drawing.Fonts.OpenFontStore s_fontStore;
        private UIPlatformWinNeutral()
        {
            LayoutFarm.UI.Clipboard.SetUIPlatform(this);

            s_fontStore = new PixelFarm.Drawing.Fonts.OpenFontStore();

            //no gdi+
            PixelFarm.Drawing.WinGdi.WinGdiFontFace.SetFontLoader(s_fontStore);
            //gles2 
            //
            PixelFarm.Drawing.GLES2.GLES2Platform.SetFontLoader(s_fontStore);
            //skia

            if (!YourImplementation.BootStrapSkia.IsNativeLibAvailable())
            {
                //set font not found handler
                PixelFarm.Drawing.Skia.SkiaGraphicsPlatform.SetFontLoader(s_fontStore);

            }
        }
        public override UITimer CreateUITimer()
        {
            return new MyUITimer();
        }
        public override void ClearClipboardData()
        {
            throw new System.NotSupportedException();
        }
        public override string GetClipboardData()
        {
            throw new System.NotSupportedException();
        }
        public override void SetClipboardData(string textData)
        {
            throw new System.NotSupportedException();
        }

        PixelFarm.Drawing.WinGdi.Gdi32IFonts _gdiPlusIFonts = new PixelFarm.Drawing.WinGdi.Gdi32IFonts();
        public PixelFarm.Drawing.IFonts GetIFonts()
        {
            return this._gdiPlusIFonts;
        }

        public static readonly UIPlatformWinNeutral platform = new UIPlatformWinNeutral();
    }
}