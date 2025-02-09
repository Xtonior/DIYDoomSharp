using DiyDoomSharp.src.DataTypes;
using SDL2;

namespace DiyDoomSharp.src
{
    public class DisplayManager
    {
        private int m_iScreenWidth;
        private int m_iScreenHeight;

        private List<WADPalette> m_ColorPalettes;

        private nint m_Window;
        private nint m_Renderer;

        private IntPtr m_Texture;

        private IntPtr m_ScreenBuffer;
        private IntPtr m_RGDBuffer;

        public DisplayManager(int windowWidth, int windowHeight)
        {
            m_iScreenWidth = windowWidth;
            m_iScreenHeight = windowHeight;
            m_Window = IntPtr.Zero;
            m_Renderer = IntPtr.Zero;
            m_ScreenBuffer = IntPtr.Zero;
            m_RGDBuffer = IntPtr.Zero;
            m_Texture = IntPtr.Zero;
            m_ColorPalettes = new List<WADPalette>();
        }

        public void Dispose()
        {
            SDL.SDL_DestroyRenderer(m_Renderer);
            SDL.SDL_DestroyWindow(m_Window);
            SDL.SDL_Quit();
        }

        public void InitFrame()
        {
            SDL.SDL_FillRect(m_ScreenBuffer, IntPtr.Zero, 0);
        }

        public void Render()
        {
            SDL.SDL_SetPaletteColors(SDL.SDL_AllocPalette(256), m_ColorPalettes[0].Colors.ToArray(), 0, 256);
            SDL.SDL_BlitSurface(m_ScreenBuffer, IntPtr.Zero, m_RGDBuffer, IntPtr.Zero);
            SDL.SDL_UpdateTexture(m_Texture, IntPtr.Zero, SDL.SDL_LockSurface(m_RGDBuffer), SDL.SDL_LockSurface(m_RGDBuffer));
            SDL.SDL_RenderCopy(m_Renderer, m_Texture, IntPtr.Zero, IntPtr.Zero);
            SDL.SDL_RenderPresent(m_Renderer);
        }

        public void AddColorPalette(WADPalette palette)
        {
            m_ColorPalettes.Add(palette);
        }

        public IntPtr GetScreenBuffer()
        {
            return m_ScreenBuffer;
        }

        public IntPtr Init(string windowTitle)
        {
            if (SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING) != 0)
            {
                Console.WriteLine($"SDL failed to initialize! SDL_Error: {SDL.SDL_GetError()}");
                return IntPtr.Zero;
            }

            m_Window = SDL.SDL_CreateWindow(windowTitle, SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, m_iScreenWidth, m_iScreenHeight, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
            if (m_Window == IntPtr.Zero)
            {
                Console.WriteLine($"SDL failed to create window! SDL_Error: {SDL.SDL_GetError()}");
                return IntPtr.Zero;
            }

            m_Renderer = SDL.SDL_CreateRenderer(m_Window, -1, 0);
            if (m_Renderer == IntPtr.Zero)
            {
                Console.WriteLine($"SDL failed to create renderer! SDL_Error: {SDL.SDL_GetError()}");
                return IntPtr.Zero;
            }

            uint pixelFormat = SDL.SDL_GetWindowPixelFormat(m_Window);
            m_ScreenBuffer = SDL.SDL_CreateRGBSurface(0, m_iScreenWidth, m_iScreenHeight, 8, 0, 0, 0, 0);
            if (m_ScreenBuffer == IntPtr.Zero)
            {
                Console.WriteLine($"SDL failed to create 8-bit surface! SDL_Error: {SDL.SDL_GetError()}");
                return IntPtr.Zero;
            }

            SDL.SDL_FillRect(m_ScreenBuffer, IntPtr.Zero, 0);

            SDL.SDL_PixelFormatEnumToMasks(pixelFormat, out int bpp, out uint rMask, out uint gMask, out uint bMask, out uint aMask);
            m_RGDBuffer = SDL.SDL_CreateRGBSurface(0, m_iScreenWidth, m_iScreenHeight, 32, rMask, gMask, bMask, aMask);
            if (m_RGDBuffer == IntPtr.Zero)
            {
                Console.WriteLine($"SDL failed to create RGB surface! SDL_Error: {SDL.SDL_GetError()}");
                return IntPtr.Zero;
            }

            SDL.SDL_FillRect(m_RGDBuffer, IntPtr.Zero, 0);
            m_Texture = SDL.SDL_CreateTexture(m_Renderer, SDL.SDL_PIXELFORMAT_ARGB8888, (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING, m_iScreenWidth, m_iScreenHeight);
            if (m_Texture == IntPtr.Zero)
            {
                Console.WriteLine($"SDL failed to create texture! SDL_Error: {SDL.SDL_GetError()}");
                return IntPtr.Zero;
            }

            return m_Renderer;
        }
    }
}
