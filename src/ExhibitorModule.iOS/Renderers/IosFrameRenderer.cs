using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ExhibitorModule.iOS.Renderers;
using CoreGraphics;

[assembly:ExportRenderer(typeof(Frame), typeof(IosFrameRenderer))]
namespace ExhibitorModule.iOS.Renderers
{
    public class IosFrameRenderer : FrameRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null || !e.NewElement.HasShadow)
                return;

            Layer.CornerRadius = Element.CornerRadius;
            Layer.ShadowOffset = new CGSize(-2, 2);
            Layer.ShadowRadius = 12;
            Layer.ShadowOpacity = 0.3f;
        }
    }
}
