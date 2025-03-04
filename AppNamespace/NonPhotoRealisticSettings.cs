namespace AppNamespace;

public class NonPhotoRealisticSettings
{
	public readonly string Name;

	public readonly bool EnableToonShading;

	public readonly bool EnableEdgeDetect;

	public readonly float EdgeWidth;

	public readonly float EdgeIntensity;

	public readonly bool EnableSketch;

	public readonly bool SketchInColor;

	public readonly float SketchThreshold;

	public readonly float SketchBrightness;

	public readonly float SketchJitterSpeed;

	public static NonPhotoRealisticSettings[] PresetSettings = new NonPhotoRealisticSettings[7]
	{
		new NonPhotoRealisticSettings("Cartoon", enableToonShading: true, enableEdgeDetect: true, 1f, 1f, enableSketch: false, sketchInColor: false, 0f, 0f, 0f),
		new NonPhotoRealisticSettings("Pencil", enableToonShading: true, enableEdgeDetect: false, 0.5f, 0.5f, enableSketch: true, sketchInColor: false, 0.1f, 0.3f, 0.05f),
		new NonPhotoRealisticSettings("Chunky Monochrome", enableToonShading: true, enableEdgeDetect: true, 1.5f, 0.5f, enableSketch: true, sketchInColor: false, 0f, 0.35f, 0f),
		new NonPhotoRealisticSettings("Colored Hatching", enableToonShading: true, enableEdgeDetect: true, 0.5f, 1f, enableSketch: true, sketchInColor: true, 0.2f, 0.5f, 0.075f),
		new NonPhotoRealisticSettings("Cartoon 2", enableToonShading: true, enableEdgeDetect: true, 1f, 1f, enableSketch: false, sketchInColor: false, 0.2f, 0.5f, 0.075f),
		new NonPhotoRealisticSettings("Subtle Edge Enhancement", enableToonShading: false, enableEdgeDetect: true, 0.5f, 0.5f, enableSketch: false, sketchInColor: false, 0f, 0f, 0f),
		new NonPhotoRealisticSettings("Nothing Special", enableToonShading: false, enableEdgeDetect: false, 0f, 0f, enableSketch: false, sketchInColor: false, 0f, 0f, 0f)
	};

	public NonPhotoRealisticSettings(string name, bool enableToonShading, bool enableEdgeDetect, float edgeWidth, float edgeIntensity, bool enableSketch, bool sketchInColor, float sketchThreshold, float sketchBrightness, float sketchJitterSpeed)
	{
		Name = name;
		EnableToonShading = enableToonShading;
		EnableEdgeDetect = enableEdgeDetect;
		EdgeWidth = edgeWidth;
		EdgeIntensity = edgeIntensity;
		EnableSketch = enableSketch;
		SketchInColor = sketchInColor;
		SketchThreshold = sketchThreshold;
		SketchBrightness = sketchBrightness;
		SketchJitterSpeed = sketchJitterSpeed;
	}
}
