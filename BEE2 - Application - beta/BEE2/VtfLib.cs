using System;
using System.Runtime.InteropServices;

internal static class VtfLib
{
    #region Wow64
    public static bool IsWow64()
    {
        return (IntPtr.Size == 8);
    }
    #endregion

    #region Constants
    public const int iVersion = 130;
    public const string sVersion = "1.3.0";

    public const uint uiMajorVersion = 7;
    public const uint uiMinorVersion = 4;
    #endregion

    #region Enumerations
    public enum Option
    {
        OptionDXTQuality = 0,

        OptionLuminanceWeightR,
        OptionLuminanceWeightG,
        OptionLuminanceWeightB,

        OptionBlueScreenMaskR,
        OptionBlueScreenMaskG,
        OptionBlueScreenMaskB,

        OptionBlueScreenClearR,
        OptionBlueScreenClearG,
        OptionBlueScreenClearB,

        OptionFP16HDRKey,
        OptionFP16HDRShift,
        OptionFP16HDRGamma,

        OptionUnsharpenRadius,
        OptionUnsharpenAmount,
        OptionUnsharpenThreshold,

        OptionXSharpenStrength,
        OptionXSharpenThreshold,

        OptionVMTParseMode
    }

    public enum ImageFormat
    {
        ImageFormatRGBA8888 = 0,
        ImageFormatABGR8888,
        ImageFormatRGB888,
        ImageFormatBGR888,
        ImageFormatRGB565,
        ImageFormatI8,
        ImageFormatIA88,
        ImageFormatP8,
        ImageFormatA8,
        ImageFormatRGB888BlueScreen,
        ImageFormatBGR888BlueScreen,
        ImageFormatARGB8888,
        ImageFormatBGRA8888,
        ImageFormatDXT1,
        ImageFormatDXT3,
        ImageFormatDXT5,
        ImageFormatBGRX8888,
        ImageFormatBGR565,
        ImageFormatBGRX5551,
        ImageFormatBGRA4444,
        ImageFormatDXT1OneBitAlpha,
        ImageFormatBGRA5551,
        ImageFormatUV88,
        ImageFormatUVWQ8888,
        ImageFormatRGBA16161616F,
        ImageFormatRGBA16161616,
        ImageFormatUVLX8888,
        ImageFormatI32F,
        ImageFormatRGB323232F,
        ImageFormatRGBA32323232F,
        ImageFormatCount,
        ImageFormatNone = -1
    }

    public enum ImageFlag : uint
    {
        ImageFlagNone = 0x00000000,
        ImageFlagPointSample = 0x00000001,
        ImageFlagTrilinear = 0x00000002,
        ImageFlagClampS = 0x00000004,
        ImageFlagClampT = 0x00000008,
        ImageFlagAnisotropic = 0x00000010,
        ImageFlagHintDXT5 = 0x00000020,
        ImageFlagSRGB = 0x00000040,
        ImageFlagNormal = 0x00000080,
        ImageFlagNoMIP = 0x00000100,
        ImageFlagNoLOD = 0x00000200,
        ImageFlagMinMIP = 0x00000400,
        ImageFlagProcedural = 0x00000800,
        ImageFlagOneBitAlpha = 0x00001000,
        ImageFlagEightBitAlpha = 0x00002000,
        ImageFlagEnviromentMap = 0x00004000,
        ImageFlagRenderTarget = 0x00008000,
        ImageFlagDepthRenderTarget = 0x00010000,
        ImageFlagNoDebugOverride = 0x00020000,
        ImageFlagSingleCopy = 0x00040000,
        ImageFlagUnused0 = 0x00080000,
        ImageFlagUnused1 = 0x00100000,
        ImageFlagUnused2 = 0x00200000,
        ImageFlagUnused3 = 0x00400000,
        ImageFlagNoDepthBuffer = 0x00800000,
        ImageFlagUnused4 = 0x01000000,
        ImageFlagClampU = 0x02000000,
        ImageFlagVertexTexture = 0x04000000,
        ImageFlagSSBump = 0x08000000,
        ImageFlagUnused5 = 0x10000000,
        ImageFlagBorder = 0x20000000,
        ImageFlagCount = 30
    }

    public enum CubemapFace
    {
        CubemapFaceRight = 0,
        CubemapFaceLeft,
        CubemapFaceBack,
        CubemapFaceFront,
        CubemapFaceUp,
        CubemapFaceDown,
        CubemapFaceSphereMap,
        CubemapFaceCount
    }

    public enum MipmapFilter
    {
        MipmapFilterPoint = 0,
        MipmapFilterBox,
        MipmapFilterTriangle,
        MipmapFilterQuadratic,
        MipmapFilterCubic,
        MipmapFilterCatrom,
        MipmapFilterMitchell,
        MipmapFilterGaussian,
        MipmapFilterSinC,
        MipmapFilterBessel,
        MipmapFilterHanning,
        MipmapFilterHamming,
        MipmapFilterBlackman,
        MipmapFilterKaiser,
        MipmapFilterCount
    }

    public enum SharpenFilter
    {
        SharpenFilterNone = 0,
        SharpenFilterNegative,
        SharpenFilterLighter,
        SharpenFilterDarker,
        SharpenFilterContrastMore,
        SharpenFilterContrastLess,
        SharpenFilterSmoothen,
        SharpenFilterSharpenSoft,
        SharpenFilterSharpenMeium,
        SharpenFilterSharpenStrong,
        SharpenFilterFindEdges,
        SharpenFilterContour,
        SharpenFilterEdgeDetect,
        SharpenFilterEdgeDetectSoft,
        SharpenFilterEmboss,
        SharpenFilterMeanRemoval,
        SharpenFilterUnsharp,
        SharpenFilterXSharpen,
        SharpenFilterWarpSharp,
        SharpenFilterCount
    }

    public enum DXTQuality
    {
        DXTQualityLow = 0,
        DXTQualityMedium,
        DXTQualityHigh,
        DXTQualityHighest,
        DXTQualityCount
    }

    public enum KernelFilter
    {
        KernelFilter4x = 0,
        KernelFilter3x3,
        KernelFilter5x5,
        KernelFilter7x7,
        KernelFilter9x9,
        KernelFilterDuDv,
        KernelFilterCount
    }

    public enum HeightConversionMethod
    {
        HeightConversionMethodAlpha = 0,
        HeightConversionMethodAverageRGB,
        HeightConversionMethodBiasedRGB,
        HeightConversionMethodRed,
        HeightConversionMethodGreed,
        HeightConversionMethodBlue,
        HeightConversionMethodMaxRGB,
        HeightConversionMethodColorSspace,
        //HeightConversionMethodNormalize,
        HeightConversionMethodCount
    }

    public enum NormalAlphaResult
    {
        NormalAlphaResultNoChange = 0,
        NormalAlphaResultHeight,
        NormalAlphaResultBlack,
        NormalAlphaResultWhite,
        NormalAlphaResultCount
    }

    public enum ResizeMethod
    {
        ResizeMethodNearestPowerTwo = 0,
        ResizeMethodBiggestPowerTwo,
        ResizeMethodSmallestPowerTwo,
        ResizeMethodSet,
        ResizeMethodCount
    }

    public enum ResourceFlag : uint
    {
        ResourceFlagNoDataChunk = 0x02,
        ResourceFlagCount = 1
    }

    public enum ResourceType : uint
    {
        ResourceTypeLowResolutionImage = 0x01,
        ResourceTypeImage = 0x30,
        ResourceTypeSheet = 0x10,
        ResourceTypeCRC = 'C' | ('R' << 8) | ('C' << 24) | (ResourceFlag.ResourceFlagNoDataChunk << 32),
        ResourceTypeLODControl = 'L' | ('O' << 8) | ('D' << 24) | (ResourceFlag.ResourceFlagNoDataChunk << 32),
        ResourceTypeTextureSettingsEx = 'T' | ('S' << 8) | ('O' << 24) | (ResourceFlag.ResourceFlagNoDataChunk << 32),
        ResourceTypeKeyValueData = 'K' | ('V' << 8) | ('D' << 24)
    }

    public enum Proc
    {
        ProcReadClose = 0,
        ProcReadOpen,
        ProcReadRead,
        ProcReadSeek,
        ProcReadTell,
        ProcReadSize,
        ProcWriteClose,
        ProcWriteOpen,
        ProcWriteWrite,
        ProcWriteSeek,
        ProcWriteSize,
        ProcWriteTell
    }

    public enum SeekMode
    {
        Begin = 0,
        Current,
        End
    }
    #endregion

    #region Structures
    public const uint uiMaximumResources = 32;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ImageFormatInfo
    {
        //[MarshalAs(UnmanagedType.LPStr)]
        public IntPtr sName;
        public uint uiBitsPerPixel;
        public uint uiBytesPerPixel;
        public uint uiRedBitsPerPixel;
        public uint uiGreenBitsPerPixel;
        public uint uiBlueBitsPerPixel;
        public uint uiAlphaBitsPerPixel;
        [MarshalAs(UnmanagedType.U1)]
        public bool bIsCompressed;
        [MarshalAs(UnmanagedType.U1)]
        public bool bIsSupported;

        public string GetName()
        {
            return sName == IntPtr.Zero ? null : Marshal.PtrToStringAnsi(sName);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CreateOptions
    {
        public uint uiVersionMajor;
        public uint uiVersionMinor;
        public ImageFormat eImageFormat;

        public uint uiFlags;
        public uint uiStartFrame;
        public float fBumpScale;
        public float fRefectivityX;
        public float fRefectivityY;
        public float fRefectivityZ;

        [MarshalAs(UnmanagedType.U1)]
        public bool bMipmaps;
        public MipmapFilter eMipmapFilter;
        public SharpenFilter eSharpenFilter;

        [MarshalAs(UnmanagedType.U1)]
        public bool bThumbnail;
        [MarshalAs(UnmanagedType.U1)]
        public bool bReflectivity;

        [MarshalAs(UnmanagedType.U1)]
        public bool bResize;
        public ResizeMethod eResizeMethod;
        public MipmapFilter eResizeFilter;
        public SharpenFilter eResizeSharpenFilter;
        public uint uiResizeWidth;
        public uint uiResizeHeight;

        [MarshalAs(UnmanagedType.U1)]
        public bool bResizeClamp;
        public uint uiResizeClampWidth;
        public uint uiResizeClampHeight;

        [MarshalAs(UnmanagedType.U1)]
        public bool bGammaCorrection;
        public float fGammaCorrection;

        [MarshalAs(UnmanagedType.U1)]
        public bool bNormalMap;
        public KernelFilter eKernelFilter;
        public HeightConversionMethod eHeightConversionMethod;
        public NormalAlphaResult eNormalAlphaResult;
        public byte uiNormalMinimumZ;
        public float fNormalScale;
        [MarshalAs(UnmanagedType.U1)]
        public bool bNormalWrap;
        [MarshalAs(UnmanagedType.U1)]
        public bool bNormalInvertX;
        [MarshalAs(UnmanagedType.U1)]
        public bool bNormalInvertY;
        [MarshalAs(UnmanagedType.U1)]
        public bool bNormalInvertZ;

        [MarshalAs(UnmanagedType.U1)]
        public bool bSphereMap;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LODControlResource
    {
        public byte uiResolutionClampU;
        public byte uiResolutionClampV;
        public byte uiPadding0;
        public byte uiPadding1;
    }
    #endregion

    #region Callback Functions
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.U1)]
    public delegate bool ReadCloseProc(IntPtr pUserData);
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.U1)]
    public delegate bool ReadOpenProc(IntPtr pUserData);
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
    public delegate uint ReadReadProc(IntPtr lpData, uint uiBytes, IntPtr pUserData);
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
    public delegate uint ReadSeekProc(int iOffset, SeekMode eSeekMode, IntPtr pUserData);
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
    public delegate uint ReadSizeProc(IntPtr pUserData);
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
    public delegate uint ReadTellProc(IntPtr pUserData);

    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.U1)]
    public delegate bool WriteCloseProc(IntPtr pUserData);
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.U1)]
    public delegate bool WriteOpenProc(IntPtr pUserData);
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
    public delegate uint WriteWriteProc(IntPtr lpData, uint uiBytes, IntPtr pUserData);
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
    public delegate uint WriteSeekProc(int iOffset, SeekMode eSeekMode, IntPtr pUserData);
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
    public delegate uint WriteSizeProc(IntPtr pUserData);
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
    public delegate uint WriteTellProc(IntPtr pUserData);
    #endregion

    #region Functions
    //
    // VTFLib
    //

    public unsafe static uint vlGetVersion()
    {
        return IsWow64() ? x64.vlGetVersion() : x86.vlGetVersion();
    }
    public unsafe static string vlGetVersionString()
    {
        return IsWow64() ? x64.vlGetVersionString() : x86.vlGetVersionString();
    }

    public unsafe static string vlGetLastError()
    {
        return IsWow64() ? x64.vlGetLastError() : x86.vlGetLastError();
    }

    public unsafe static bool vlInitialize()
    {
        return IsWow64() ? x64.vlInitialize() : x86.vlInitialize();
    }
    public unsafe static void vlShutdown()
    {
        if (IsWow64()) x64.vlShutdown(); else x86.vlShutdown();
    }

    public unsafe static bool vlGetBoolean(Option eOption)
    {
        return IsWow64() ? x64.vlGetBoolean(eOption) : x86.vlGetBoolean(eOption);
    }
    public unsafe static void vlSetBoolean(Option eOption, bool bValue)
    {
        if (IsWow64()) x64.vlSetBoolean(eOption, bValue); else x86.vlSetBoolean(eOption, bValue);
    }

    public unsafe static int vlGetInteger(Option eOption)
    {
        return IsWow64() ? x64.vlGetInteger(eOption) : x86.vlGetInteger(eOption);
    }
    public unsafe static void vlSetInteger(Option eOption, int iValue)
    {
        if (IsWow64()) x64.vlSetInteger(eOption, iValue); else x86.vlSetInteger(eOption, iValue);
    }

    public unsafe static float vlGetFloat(Option eOption)
    {
        return IsWow64() ? x64.vlGetFloat(eOption) : x86.vlGetFloat(eOption);
    }
    public unsafe static void vlSetFloat(Option eOption, float fValue)
    {
        if (IsWow64()) x64.vlSetFloat(eOption, fValue); else x86.vlSetFloat(eOption, fValue);
    }

    //
    // Proc
    //

    public unsafe static IntPtr vlGetProc(Proc eProc)
    {
        return IsWow64() ? x64.vlGetProc(eProc) : x86.vlGetProc(eProc);
    }
    public unsafe static void vlSetProc(Proc eProc, IntPtr pProc)
    {
        if (IsWow64()) x64.vlSetProc(eProc, pProc); else x86.vlSetProc(eProc, pProc);
    }

    //
    // Memory managment routines.
    //

    public unsafe static bool vlImageIsBound()
    {
        return IsWow64() ? x64.vlImageIsBound() : x86.vlImageIsBound();
    }
    public unsafe static bool vlBindImage(uint uiImage)
    {
        return IsWow64() ? x64.vlBindImage(uiImage) : x86.vlBindImage(uiImage);
    }

    public unsafe static bool vlCreateImage(uint* uiImage)
    {
        return IsWow64() ? x64.vlCreateImage(uiImage) : x86.vlCreateImage(uiImage);
    }
    public unsafe static void vlDeleteImage(uint uiImage)
    {
        if (IsWow64()) x64.vlDeleteImage(uiImage); else x86.vlDeleteImage(uiImage);
    }

    //
    // Library routines.  (Basically class wrappers.)
    //

    public unsafe static void vlImageCreateDefaultCreateStructure(out CreateOptions CreateOptions)
    {
        if (IsWow64()) x64.vlImageCreateDefaultCreateStructure(out CreateOptions); else x86.vlImageCreateDefaultCreateStructure(out CreateOptions);
    }

    public unsafe static bool vlImageCreate(uint uiWidth, uint uiHeight, uint uiFrames, uint uiFaces, uint uiSlices, ImageFormat ImageFormat, bool bThumbnail, bool bMipmaps, bool bNullImageData)
    {
        return IsWow64() ? x64.vlImageCreate(uiWidth, uiHeight, uiFrames, uiFaces, uiSlices, ImageFormat, bThumbnail, bMipmaps, bNullImageData) : x86.vlImageCreate(uiWidth, uiHeight, uiFrames, uiFaces, uiSlices, ImageFormat, bThumbnail, bMipmaps, bNullImageData);
    }
    public unsafe static bool vlImageCreateSingle(uint uiWidth, uint uiHeight, byte* lpImageDataRGBA8888, ref CreateOptions CreateOptions)
    {
        return IsWow64() ? x64.vlImageCreateSingle(uiWidth, uiHeight, lpImageDataRGBA8888, ref CreateOptions) : x86.vlImageCreateSingle(uiWidth, uiHeight, lpImageDataRGBA8888, ref CreateOptions);
    }
    public unsafe static bool vlImageCreateMultiple(uint uiWidth, uint uiHeight, uint uiFrames, uint uiFaces, uint uiSlices, byte** lpImageDataRGBA8888, ref CreateOptions CreateOptions)
    {
        return IsWow64() ? x64.vlImageCreateMultiple(uiWidth, uiHeight, uiFrames, uiFaces, uiSlices, lpImageDataRGBA8888, ref CreateOptions) : x86.vlImageCreateMultiple(uiWidth, uiHeight, uiFrames, uiFaces, uiSlices, lpImageDataRGBA8888, ref CreateOptions);
    }
    public unsafe static void vlImageDestroy()
    {
        if (IsWow64()) x64.vlImageDestroy(); else x86.vlImageDestroy();
    }

    public unsafe static bool vlImageIsLoaded()
    {
        return IsWow64() ? x64.vlImageIsLoaded() : x86.vlImageIsLoaded();
    }

    public unsafe static bool vlImageLoad(string sFileName, bool bHeaderOnly)
    {
        return IsWow64() ? x64.vlImageLoad(sFileName, bHeaderOnly) : x86.vlImageLoad(sFileName, bHeaderOnly);
    }
    public unsafe static bool vlImageLoadLump(void* lpData, uint uiBufferSize, bool bHeaderOnly)
    {
        return IsWow64() ? x64.vlImageLoadLump(lpData, uiBufferSize, bHeaderOnly) : x86.vlImageLoadLump(lpData, uiBufferSize, bHeaderOnly);
    }
    public unsafe static bool vlImageLoadProc(IntPtr pUserData, bool bHeaderOnly)
    {
        return IsWow64() ? x64.vlImageLoadProc(pUserData, bHeaderOnly) : x86.vlImageLoadProc(pUserData, bHeaderOnly);
    }

    public unsafe static bool vlImageSave(string sFileName)
    {
        return IsWow64() ? x64.vlImageSave(sFileName) : x86.vlImageSave(sFileName);
    }
    public unsafe static bool vlImageSaveLump(void* lpData, uint uiBufferSize, uint* uiSize)
    {
        return IsWow64() ? x64.vlImageSaveLump(lpData, uiBufferSize, uiSize) : x86.vlImageSaveLump(lpData, uiBufferSize, uiSize);
    }

    public unsafe static bool vlImageSaveProc(IntPtr pUserData)
    {
        return IsWow64() ? x64.vlImageSaveProc(pUserData) : x86.vlImageSaveProc(pUserData);
    }

    //
    // Image routines.
    //

    public unsafe static uint vlImageGetHasImage()
    {
        return IsWow64() ? x64.vlImageGetHasImage() : x86.vlImageGetHasImage();
    }

    public unsafe static uint vlImageGetMajorVersion()
    {
        return IsWow64() ? x64.vlImageGetMajorVersion() : x86.vlImageGetMajorVersion();
    }
    public unsafe static uint vlImageGetMinorVersion()
    {
        return IsWow64() ? x64.vlImageGetMinorVersion() : x86.vlImageGetMinorVersion();
    }

    public unsafe static uint vlImageGetSize()
    {
        return IsWow64() ? x64.vlImageGetSize() : x86.vlImageGetSize();
    }
    public unsafe static uint vlImageGetWidth()
    {
        return IsWow64() ? x64.vlImageGetWidth() : x86.vlImageGetWidth();
    }
    public unsafe static uint vlImageGetHeight()
    {
        return IsWow64() ? x64.vlImageGetHeight() : x86.vlImageGetHeight();
    }
    public unsafe static uint vlImageGetDepth()
    {
        return IsWow64() ? x64.vlImageGetDepth() : x86.vlImageGetDepth();
    }

    public unsafe static uint vlImageGetFrameCount()
    {
        return IsWow64() ? x64.vlImageGetFrameCount() : x86.vlImageGetFrameCount();
    }
    public unsafe static uint vlImageGetFaceCount()
    {
        return IsWow64() ? x64.vlImageGetFaceCount() : x86.vlImageGetFaceCount();
    }
    public unsafe static uint vlImageGetMipmapCount()
    {
        return IsWow64() ? x64.vlImageGetMipmapCount() : x86.vlImageGetMipmapCount();
    }

    public unsafe static uint vlImageGetStartFrame()
    {
        return IsWow64() ? x64.vlImageGetStartFrame() : x86.vlImageGetStartFrame();
    }
    public unsafe static void vlImageSetStartFrame(uint uiStartFrame)
    {
        if (IsWow64()) x64.vlImageSetStartFrame(uiStartFrame); else x86.vlImageSetStartFrame(uiStartFrame);
    }

    public unsafe static uint vlImageGetFlags()
    {
        return IsWow64() ? x64.vlImageGetFlags() : x86.vlImageGetFlags();
    }
    public unsafe static void vlImageSetFlags(uint uiFlags)
    {
        if (IsWow64()) x64.vlImageSetFlags(uiFlags); else x86.vlImageSetFlags(uiFlags);
    }


    public unsafe static bool vlImageGetFlag(ImageFlag ImageFlag)
    {
        return IsWow64() ? x64.vlImageGetFlag(ImageFlag) : x86.vlImageGetFlag(ImageFlag);
    }
    public unsafe static void vlImageSetFlag(ImageFlag ImageFlag, bool bState)
    {
        if (IsWow64()) x64.vlImageSetFlag(ImageFlag, bState); else x86.vlImageSetFlag(ImageFlag, bState);
    }

    public unsafe static float vlImageGetBumpmapScale()
    {
        return IsWow64() ? x64.vlImageGetBumpmapScale() : x86.vlImageGetBumpmapScale();
    }
    public unsafe static void vlImageSetBumpmapScale(float fBumpmapScale)
    {
        if (IsWow64()) x64.vlImageSetBumpmapScale(fBumpmapScale); else x86.vlImageSetBumpmapScale(fBumpmapScale);
    }

    public unsafe static void vlImageGetReflectivity(float* fRed, float* fGreen, float* fBlue)
    {
        if (IsWow64()) x64.vlImageGetReflectivity(fRed, fGreen, fBlue); else x86.vlImageGetReflectivity(fRed, fGreen, fBlue);
    }
    public unsafe static void vlImageSetReflectivity(float fRed, float fGreen, float fBlue)
    {
        if (IsWow64()) x64.vlImageSetReflectivity(fRed, fGreen, fBlue); else x86.vlImageSetReflectivity(fRed, fGreen, fBlue);
    }

    public unsafe static ImageFormat vlImageGetFormat()
    {
        return IsWow64() ? x64.vlImageGetFormat() : x86.vlImageGetFormat();
    }

    public unsafe static byte* vlImageGetData(uint uiFrame, uint uiFace, uint uiSlice, uint uiMipmapLevel)
    {
        return IsWow64() ? x64.vlImageGetData(uiFrame, uiFace, uiSlice, uiMipmapLevel) : x86.vlImageGetData(uiFrame, uiFace, uiSlice, uiMipmapLevel);
    }
    public unsafe static void vlImageSetData(uint uiFrame, uint uiFace, uint uiSlice, uint uiMipmapLevel, byte* lpData)
    {
        if (IsWow64()) x64.vlImageSetData(uiFrame, uiFace, uiSlice, uiMipmapLevel, lpData); else x86.vlImageSetData(uiFrame, uiFace, uiSlice, uiMipmapLevel, lpData);
    }

    //
    // Thumbnail routines.
    //

    public unsafe static bool vlImageGetHasThumbnail()
    {
        return IsWow64() ? x64.vlImageGetHasThumbnail() : x86.vlImageGetHasThumbnail();
    }

    public unsafe static uint vlImageGetThumbnailWidth()
    {
        return IsWow64() ? x64.vlImageGetThumbnailWidth() : x86.vlImageGetThumbnailWidth();
    }
    public unsafe static uint vlImageGetThumbnailHeight()
    {
        return IsWow64() ? x64.vlImageGetThumbnailHeight() : x86.vlImageGetThumbnailHeight();
    }

    public unsafe static ImageFormat vlImageGetThumbnailFormat()
    {
        return IsWow64() ? x64.vlImageGetThumbnailFormat() : x86.vlImageGetThumbnailFormat();
    }

    public unsafe static byte* vlImageGetThumbnailData()
    {
        return IsWow64() ? x64.vlImageGetThumbnailData() : x86.vlImageGetThumbnailData();
    }
    public unsafe static void vlImageSetThumbnailData(byte* lpData)
    {
        if (IsWow64()) x64.vlImageSetThumbnailData(lpData); else x86.vlImageSetThumbnailData(lpData);
    }

    //
    // Resource routines.
    //

    public unsafe static bool vlImageGetSupportsResources()
    {
        return IsWow64() ? x64.vlImageGetSupportsResources() : x86.vlImageGetSupportsResources();
    }

    public unsafe static uint vlImageGetResourceCount()
    {
        return IsWow64() ? x64.vlImageGetResourceCount() : x86.vlImageGetResourceCount();
    }
    public unsafe static uint vlImageGetResourceType(uint uiIndex)
    {
        return IsWow64() ? x64.vlImageGetResourceType(uiIndex) : x86.vlImageGetResourceType(uiIndex);
    }

    public unsafe static bool vlImageGetHasResource(ResourceType ResourceType)
    {
        return IsWow64() ? x64.vlImageGetHasResource(ResourceType) : x86.vlImageGetHasResource(ResourceType);
    }

    public unsafe static void* vlImageGetResourceData(ResourceType ResourceType, uint* uiSize)
    {
        return IsWow64() ? x64.vlImageGetResourceData(ResourceType, uiSize) : x86.vlImageGetResourceData(ResourceType, uiSize);
    }
    public unsafe static void* vlImageSetResourceData(ResourceType ResourceType, uint uiSize, void* lpData)
    {
        return IsWow64() ? x64.vlImageSetResourceData(ResourceType, uiSize, lpData) : x86.vlImageSetResourceData(ResourceType, uiSize, lpData);
    }

    //
    // Helper routines.
    //

    public unsafe static bool vlImageGenerateMipmaps(uint uiFace, uint uiFrame, MipmapFilter MipmapFilter, SharpenFilter SharpenFilter)
    {
        return IsWow64() ? x64.vlImageGenerateMipmaps(uiFace, uiFrame, MipmapFilter, SharpenFilter) : x86.vlImageGenerateMipmaps(uiFace, uiFrame, MipmapFilter, SharpenFilter);
    }
    public unsafe static bool vlImageGenerateAllMipmaps(MipmapFilter MipmapFilter, SharpenFilter SharpenFilter)
    {
        return IsWow64() ? x64.vlImageGenerateAllMipmaps(MipmapFilter, SharpenFilter) : x86.vlImageGenerateAllMipmaps(MipmapFilter, SharpenFilter);
    }

    public unsafe static bool vlImageGenerateThumbnail()
    {
        return IsWow64() ? x64.vlImageGenerateThumbnail() : x86.vlImageGenerateThumbnail();
    }

    public unsafe static bool vlImageGenerateNormalMap(uint uiFrame, KernelFilter KernelFilter, HeightConversionMethod HeightConversionMethod, NormalAlphaResult NormalAlphaResult)
    {
        return IsWow64() ? x64.vlImageGenerateNormalMap(uiFrame, KernelFilter, HeightConversionMethod, NormalAlphaResult) : x86.vlImageGenerateNormalMap(uiFrame, KernelFilter, HeightConversionMethod, NormalAlphaResult);
    }
    public unsafe static bool vlImageGenerateAllNormalMaps(KernelFilter KernelFilter, HeightConversionMethod HeightConversionMethod, NormalAlphaResult NormalAlphaResult)
    {
        return IsWow64() ? x64.vlImageGenerateAllNormalMaps(KernelFilter, HeightConversionMethod, NormalAlphaResult) : x86.vlImageGenerateAllNormalMaps(KernelFilter, HeightConversionMethod, NormalAlphaResult);
    }

    public unsafe static bool vlImageGenerateSphereMap()
    {
        return IsWow64() ? x64.vlImageGenerateSphereMap() : x86.vlImageGenerateSphereMap();
    }

    public unsafe static bool vlImageComputeReflectivity()
    {
        return IsWow64() ? x64.vlImageComputeReflectivity() : x86.vlImageComputeReflectivity();
    }

    //
    // Conversion routines.
    //

    public unsafe static bool vlImageGetImageFormatInfoEx(ImageFormat ImageFormat, out ImageFormatInfo ImageFormatInfo)
    {
        return IsWow64() ? x64.vlImageGetImageFormatInfoEx(ImageFormat, out ImageFormatInfo) : x86.vlImageGetImageFormatInfoEx(ImageFormat, out ImageFormatInfo);
    }

    public unsafe static uint vlImageComputeImageSize(uint uiWidth, uint uiHeight, uint uiDepth, uint uiMipmaps, ImageFormat ImageFormat)
    {
        return IsWow64() ? x64.vlImageComputeImageSize(uiWidth, uiHeight, uiDepth, uiMipmaps, ImageFormat) : x86.vlImageComputeImageSize(uiWidth, uiHeight, uiDepth, uiMipmaps, ImageFormat);
    }

    public unsafe static uint vlImageComputeMipmapCount(uint uiWidth, uint uiHeight, uint uiDepth)
    {
        return IsWow64() ? x64.vlImageComputeMipmapCount(uiWidth, uiHeight, uiDepth) : x86.vlImageComputeMipmapCount(uiWidth, uiHeight, uiDepth);
    }
    public unsafe static void vlImageComputeMipmapDimensions(uint uiWidth, uint uiHeight, uint uiDepth, uint uiMipmapLevel, uint* uiMipmapWidth, uint* uiMipmapHeight, uint* uiMipmapDepth)
    {
        if (IsWow64()) x64.vlImageComputeMipmapDimensions(uiWidth, uiHeight, uiDepth, uiMipmapLevel, uiMipmapWidth, uiMipmapHeight, uiMipmapDepth); else x86.vlImageComputeMipmapDimensions(uiWidth, uiHeight, uiDepth, uiMipmapLevel, uiMipmapWidth, uiMipmapHeight, uiMipmapDepth);
    }
    public unsafe static uint vlImageComputeMipmapSize(uint uiWidth, uint uiHeight, uint uiDepth, uint uiMipmapLevel, ImageFormat ImageFormat)
    {
        return IsWow64() ? x64.vlImageComputeMipmapSize(uiWidth, uiHeight, uiDepth, uiMipmapLevel, ImageFormat) : x86.vlImageComputeMipmapSize(uiWidth, uiHeight, uiDepth, uiMipmapLevel, ImageFormat);
    }

    public unsafe static bool vlImageConvert(byte* lpSource, byte* lpDest, uint uiWidth, uint uiHeight, ImageFormat SourceFormat, ImageFormat DestFormat)
    {
        return IsWow64() ? x64.vlImageConvert(lpSource, lpDest, uiWidth, uiHeight, SourceFormat, DestFormat) : x86.vlImageConvert(lpSource, lpDest, uiWidth, uiHeight, SourceFormat, DestFormat);
    }

    public unsafe static bool vlImageConvertToNormalMap(byte* lpSourceRGBA8888, byte* lpDestRGBA8888, uint uiWidth, uint uiHeight, KernelFilter KernelFilter, HeightConversionMethod HeightConversionMethod, NormalAlphaResult NormalAlphaResult, byte bMinimumZ, float fScale, bool bWrap, bool bInvertX, bool bInvertY)
    {
        return IsWow64() ? x64.vlImageConvertToNormalMap(lpSourceRGBA8888, lpDestRGBA8888, uiWidth, uiHeight, KernelFilter, HeightConversionMethod, NormalAlphaResult, bMinimumZ, fScale, bWrap, bInvertX, bInvertY) : x86.vlImageConvertToNormalMap(lpSourceRGBA8888, lpDestRGBA8888, uiWidth, uiHeight, KernelFilter, HeightConversionMethod, NormalAlphaResult, bMinimumZ, fScale, bWrap, bInvertX, bInvertY);
    }

    public unsafe static bool vlImageResize(byte* lpSourceRGBA8888, byte* lpDestRGBA8888, uint uiSourceWidth, uint uiSourceHeight, uint uiDestWidth, uint uiDestHeight, MipmapFilter ResizeFilter, SharpenFilter SharpenFilter)
    {
        return IsWow64() ? x64.vlImageResize(lpSourceRGBA8888, lpDestRGBA8888, uiSourceWidth, uiSourceHeight, uiDestWidth, uiDestHeight, ResizeFilter, SharpenFilter) : x86.vlImageResize(lpSourceRGBA8888, lpDestRGBA8888, uiSourceWidth, uiSourceHeight, uiDestWidth, uiDestHeight, ResizeFilter, SharpenFilter);
    }

    public unsafe static void vlImageCorrectImageGamma(byte* lpImageDataRGBA8888, uint uiWidth, uint uiHeight, float fGammaCorrection)
    {
        if (IsWow64()) x64.vlImageCorrectImageGamma(lpImageDataRGBA8888, uiWidth, uiHeight, fGammaCorrection); else x86.vlImageCorrectImageGamma(lpImageDataRGBA8888, uiWidth, uiHeight, fGammaCorrection);
    }
    public unsafe static void vlImageComputeImageReflectivity(byte* lpImageDataRGBA8888, uint uiWidth, uint uiHeight, float* sX, float* sY, float* sZ)
    {
        if (IsWow64()) x64.vlImageComputeImageReflectivity(lpImageDataRGBA8888, uiWidth, uiHeight, sX, sY, sZ); else x86.vlImageComputeImageReflectivity(lpImageDataRGBA8888, uiWidth, uiHeight, sX, sY, sZ);
    }

    public unsafe static void vlImageFlipImage(byte* lpImageDataRGBA8888, uint uiWidth, uint uiHeight)
    {
        if (IsWow64()) x64.vlImageFlipImage(lpImageDataRGBA8888, uiWidth, uiHeight); else x86.vlImageFlipImage(lpImageDataRGBA8888, uiWidth, uiHeight);
    }
    public unsafe static void vlImageMirrorImage(byte* lpImageDataRGBA8888, uint uiWidth, uint uiHeight)
    {
        if (IsWow64()) x64.vlImageMirrorImage(lpImageDataRGBA8888, uiWidth, uiHeight); else x86.vlImageMirrorImage(lpImageDataRGBA8888, uiWidth, uiHeight);
    }

    private static class x86
    {
        //
        // VTFLib
        //

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlGetVersion();
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern string vlGetVersionString();

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern string vlGetLastError();

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlInitialize();
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlShutdown();

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlGetBoolean(Option eOption);
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlSetBoolean(Option eOption, [MarshalAs(UnmanagedType.U1)]bool bValue);

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern int vlGetInteger(Option eOption);
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlSetInteger(Option eOption, int iValue);

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern float vlGetFloat(Option eOption);
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlSetFloat(Option eOption, float fValue);

        //
        // Proc
        //

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlSetProc(Proc eProc, IntPtr pProc);
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern IntPtr vlGetProc(Proc eProc);

        //
        // Memory managment routines.
        //

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageIsBound();
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlBindImage(uint uiImage);

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlCreateImage(uint* uiImage);
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlDeleteImage(uint uiImage);

        //
        // Library routines.  (Basically class wrappers.)
        //

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageCreateDefaultCreateStructure(out CreateOptions CreateOptions);

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageCreate(uint uiWidth, uint uiHeight, uint uiFrames, uint uiFaces, uint uiSlices, ImageFormat ImageFormat, [MarshalAs(UnmanagedType.U1)]bool bThumbnail, [MarshalAs(UnmanagedType.U1)]bool bMipmaps, [MarshalAs(UnmanagedType.U1)]bool bNullImageData);
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageCreateSingle(uint uiWidth, uint uiHeight, byte* lpImageDataRGBA8888, ref CreateOptions CreateOptions);
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageCreateMultiple(uint uiWidth, uint uiHeight, uint uiFrames, uint uiFaces, uint uiSlices, byte** lpImageDataRGBA8888, ref CreateOptions CreateOptions);
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageDestroy();

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageIsLoaded();

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageLoad(string sFileName, [MarshalAs(UnmanagedType.U1)]bool bHeaderOnly);
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageLoadLump(void* lpData, uint uiBufferSize, [MarshalAs(UnmanagedType.U1)]bool bHeaderOnly);
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageLoadProc(IntPtr pUserData, [MarshalAs(UnmanagedType.U1)]bool bHeaderOnly);

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageSave(string sFileName);
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageSaveLump(void* lpData, uint uiBufferSize, uint* uiSize);
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageSaveProc(IntPtr pUserData);

        //
        // Image routines.
        //

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetHasImage();

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetMajorVersion();
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetMinorVersion();

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetSize();
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetWidth();
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetHeight();
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetDepth();

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetFrameCount();
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetFaceCount();
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetMipmapCount();

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetStartFrame();
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageSetStartFrame(uint uiStartFrame);

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetFlags();
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageSetFlags(uint uiFlags);

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageGetFlag(ImageFlag ImageFlag);
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageSetFlag(ImageFlag ImageFlag, [MarshalAs(UnmanagedType.U1)]bool bState);

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern float vlImageGetBumpmapScale();
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageSetBumpmapScale(float fBumpmapScale);

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageGetReflectivity(float* fRed, float* fGreen, float* fBlue);
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageSetReflectivity(float fRed, float fGreen, float fBlue);

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern ImageFormat vlImageGetFormat();

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern byte* vlImageGetData(uint uiFrame, uint uiFace, uint uiSlice, uint uiMipmapLevel);
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageSetData(uint uiFrame, uint uiFace, uint uiSlice, uint uiMipmapLevel, byte* lpData);

        //
        // Thumbnail routines.
        //

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageGetHasThumbnail();

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetThumbnailWidth();
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetThumbnailHeight();

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern ImageFormat vlImageGetThumbnailFormat();

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern byte* vlImageGetThumbnailData();
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageSetThumbnailData(byte* lpData);

        //
        // Resource routines.
        //

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageGetSupportsResources();

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetResourceCount();
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetResourceType(uint uiIndex);
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageGetHasResource(ResourceType ResourceType);

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void* vlImageGetResourceData(ResourceType ResourceType, uint* uiSize);
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void* vlImageSetResourceData(ResourceType ResourceType, uint uiSize, void* lpData);

        //
        // Helper routines.
        //

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageGenerateMipmaps(uint uiFace, uint uiFrame, MipmapFilter MipmapFilter, SharpenFilter SharpenFilter);
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageGenerateAllMipmaps(MipmapFilter MipmapFilter, SharpenFilter SharpenFilter);

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageGenerateThumbnail();

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageGenerateNormalMap(uint uiFrame, KernelFilter KernelFilter, HeightConversionMethod HeightConversionMethod, NormalAlphaResult NormalAlphaResult);
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageGenerateAllNormalMaps(KernelFilter KernelFilter, HeightConversionMethod HeightConversionMethod, NormalAlphaResult NormalAlphaResult);

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageGenerateSphereMap();

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageComputeReflectivity();

        //
        // Conversion routines.
        //

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageGetImageFormatInfoEx(ImageFormat ImageFormat, out ImageFormatInfo ImageFormatInfo);

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageComputeImageSize(uint uiWidth, uint uiHeight, uint uiDepth, uint uiMipmaps, ImageFormat ImageFormat);

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageComputeMipmapCount(uint uiWidth, uint uiHeight, uint uiDepth);
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageComputeMipmapDimensions(uint uiWidth, uint uiHeight, uint uiDepth, uint uiMipmapLevel, uint* uiMipmapWidth, uint* uiMipmapHeight, uint* uiMipmapDepth);
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageComputeMipmapSize(uint uiWidth, uint uiHeight, uint uiDepth, uint uiMipmapLevel, ImageFormat ImageFormat);

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageConvert(byte* lpSource, byte* lpDest, uint uiWidth, uint uiHeight, ImageFormat SourceFormat, ImageFormat DestFormat);

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageConvertToNormalMap(byte* lpSourceRGBA8888, byte* lpDestRGBA8888, uint uiWidth, uint uiHeight, KernelFilter KernelFilter, HeightConversionMethod HeightConversionMethod, NormalAlphaResult NormalAlphaResult, byte bMinimumZ, float fScale, [MarshalAs(UnmanagedType.U1)]bool bWrap, [MarshalAs(UnmanagedType.U1)]bool bInvertX, [MarshalAs(UnmanagedType.U1)]bool bInvertY);

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageResize(byte* lpSourceRGBA8888, byte* lpDestRGBA8888, uint uiSourceWidth, uint uiSourceHeight, uint uiDestWidth, uint uiDestHeight, MipmapFilter ResizeFilter, SharpenFilter SharpenFilter);

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageCorrectImageGamma(byte* lpImageDataRGBA8888, uint uiWidth, uint uiHeight, float fGammaCorrection);
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageComputeImageReflectivity(byte* lpImageDataRGBA8888, uint uiWidth, uint uiHeight, float* sX, float* sY, float* sZ);

        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageFlipImage(byte* lpImageDataRGBA8888, uint uiWidth, uint uiHeight);
        [DllImport("VTFLib.x86.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageMirrorImage(byte* lpImageDataRGBA8888, uint uiWidth, uint uiHeight);
    }

    private static class x64
    {
        //
        // VTFLib
        //

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlGetVersion();
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern string vlGetVersionString();

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern string vlGetLastError();

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlInitialize();
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlShutdown();

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlGetBoolean(Option eOption);
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlSetBoolean(Option eOption, [MarshalAs(UnmanagedType.U1)]bool bValue);

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern int vlGetInteger(Option eOption);
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlSetInteger(Option eOption, int iValue);

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern float vlGetFloat(Option eOption);
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlSetFloat(Option eOption, float fValue);

        //
        // Proc
        //

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlSetProc(Proc eProc, IntPtr pProc);
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern IntPtr vlGetProc(Proc eProc);

        //
        // Memory managment routines.
        //

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageIsBound();
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlBindImage(uint uiImage);

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlCreateImage(uint* uiImage);
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlDeleteImage(uint uiImage);

        //
        // Library routines.  (Basically class wrappers.)
        //

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageCreateDefaultCreateStructure(out CreateOptions CreateOptions);

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageCreate(uint uiWidth, uint uiHeight, uint uiFrames, uint uiFaces, uint uiSlices, ImageFormat ImageFormat, [MarshalAs(UnmanagedType.U1)]bool bThumbnail, [MarshalAs(UnmanagedType.U1)]bool bMipmaps, [MarshalAs(UnmanagedType.U1)]bool bNullImageData);
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageCreateSingle(uint uiWidth, uint uiHeight, byte* lpImageDataRGBA8888, ref CreateOptions CreateOptions);
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageCreateMultiple(uint uiWidth, uint uiHeight, uint uiFrames, uint uiFaces, uint uiSlices, byte** lpImageDataRGBA8888, ref CreateOptions CreateOptions);
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageDestroy();

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageIsLoaded();

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageLoad(string sFileName, [MarshalAs(UnmanagedType.U1)]bool bHeaderOnly);
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageLoadLump(void* lpData, uint uiBufferSize, [MarshalAs(UnmanagedType.U1)]bool bHeaderOnly);
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageLoadProc(IntPtr pUserData, [MarshalAs(UnmanagedType.U1)]bool bHeaderOnly);

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageSave(string sFileName);
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageSaveLump(void* lpData, uint uiBufferSize, uint* uiSize);
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageSaveProc(IntPtr pUserData);

        //
        // Image routines.
        //

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetHasImage();

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetMajorVersion();
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetMinorVersion();

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetSize();
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetWidth();
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetHeight();
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetDepth();

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetFrameCount();
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetFaceCount();
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetMipmapCount();

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetStartFrame();
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageSetStartFrame(uint uiStartFrame);

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetFlags();
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageSetFlags(uint uiFlags);

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageGetFlag(ImageFlag ImageFlag);
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageSetFlag(ImageFlag ImageFlag, [MarshalAs(UnmanagedType.U1)]bool bState);

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern float vlImageGetBumpmapScale();
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageSetBumpmapScale(float fBumpmapScale);

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageGetReflectivity(float* fRed, float* fGreen, float* fBlue);
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageSetReflectivity(float fRed, float fGreen, float fBlue);

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern ImageFormat vlImageGetFormat();

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern byte* vlImageGetData(uint uiFrame, uint uiFace, uint uiSlice, uint uiMipmapLevel);
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageSetData(uint uiFrame, uint uiFace, uint uiSlice, uint uiMipmapLevel, byte* lpData);

        //
        // Thumbnail routines.
        //

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageGetHasThumbnail();

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetThumbnailWidth();
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetThumbnailHeight();

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern ImageFormat vlImageGetThumbnailFormat();

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern byte* vlImageGetThumbnailData();
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageSetThumbnailData(byte* lpData);

        //
        // Resource routines.
        //

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageGetSupportsResources();

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetResourceCount();
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageGetResourceType(uint uiIndex);
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageGetHasResource(ResourceType ResourceType);

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void* vlImageGetResourceData(ResourceType ResourceType, uint* uiSize);
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void* vlImageSetResourceData(ResourceType ResourceType, uint uiSize, void* lpData);

        //
        // Helper routines.
        //

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageGenerateMipmaps(uint uiFace, uint uiFrame, MipmapFilter MipmapFilter, SharpenFilter SharpenFilter);
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageGenerateAllMipmaps(MipmapFilter MipmapFilter, SharpenFilter SharpenFilter);

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageGenerateThumbnail();

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageGenerateNormalMap(uint uiFrame, KernelFilter KernelFilter, HeightConversionMethod HeightConversionMethod, NormalAlphaResult NormalAlphaResult);
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageGenerateAllNormalMaps(KernelFilter KernelFilter, HeightConversionMethod HeightConversionMethod, NormalAlphaResult NormalAlphaResult);

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageGenerateSphereMap();

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageComputeReflectivity();

        //
        // Conversion routines.
        //

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageGetImageFormatInfoEx(ImageFormat ImageFormat, out ImageFormatInfo ImageFormatInfo);

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageComputeImageSize(uint uiWidth, uint uiHeight, uint uiDepth, uint uiMipmaps, ImageFormat ImageFormat);

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageComputeMipmapCount(uint uiWidth, uint uiHeight, uint uiDepth);
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageComputeMipmapDimensions(uint uiWidth, uint uiHeight, uint uiDepth, uint uiMipmapLevel, uint* uiMipmapWidth, uint* uiMipmapHeight, uint* uiMipmapDepth);
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint vlImageComputeMipmapSize(uint uiWidth, uint uiHeight, uint uiDepth, uint uiMipmapLevel, ImageFormat ImageFormat);

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageConvert(byte* lpSource, byte* lpDest, uint uiWidth, uint uiHeight, ImageFormat SourceFormat, ImageFormat DestFormat);

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageConvertToNormalMap(byte* lpSourceRGBA8888, byte* lpDestRGBA8888, uint uiWidth, uint uiHeight, KernelFilter KernelFilter, HeightConversionMethod HeightConversionMethod, NormalAlphaResult NormalAlphaResult, byte bMinimumZ, float fScale, [MarshalAs(UnmanagedType.U1)]bool bWrap, [MarshalAs(UnmanagedType.U1)]bool bInvertX, [MarshalAs(UnmanagedType.U1)]bool bInvertY);

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe static extern bool vlImageResize(byte* lpSourceRGBA8888, byte* lpDestRGBA8888, uint uiSourceWidth, uint uiSourceHeight, uint uiDestWidth, uint uiDestHeight, MipmapFilter ResizeFilter, SharpenFilter SharpenFilter);

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageCorrectImageGamma(byte* lpImageDataRGBA8888, uint uiWidth, uint uiHeight, float fGammaCorrection);
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageComputeImageReflectivity(byte* lpImageDataRGBA8888, uint uiWidth, uint uiHeight, float* sX, float* sY, float* sZ);

        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageFlipImage(byte* lpImageDataRGBA8888, uint uiWidth, uint uiHeight);
        [DllImport("VTFLib.x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void vlImageMirrorImage(byte* lpImageDataRGBA8888, uint uiWidth, uint uiHeight);
    }
    #endregion
}
