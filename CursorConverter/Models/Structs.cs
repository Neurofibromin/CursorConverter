using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursorConverter.Models
{
    public enum eOperation
    {
        OPERATION_CREATE = 0,
        OPERATION_EXTRACT = 1,
    }

    public enum eHyprcursorResizeAlgo
    {
        HC_RESIZE_INVALID = 0,
        HC_RESIZE_NONE,
        HC_RESIZE_BILINEAR,
        HC_RESIZE_NEAREST,
    };

    public struct SCursorTheme
    {
        public List<SCursorShape> shapes;
    }
    public struct SCursorShape
    {
        public string directory;
        public float hotspotX = 0;
        public float hotspotY = 0;
        public eHyprcursorResizeAlgo resizeAlgo = eHyprcursorResizeAlgo.HC_RESIZE_NEAREST;
        public List<SCursorImage> images;
        public List<string> overrides;
        public eShapeType shapeType = eShapeType.SHAPE_INVALID;

        public SCursorShape()
        {
        }

        public SCursorShape(string directory, float hotspotX, float hotspotY, eHyprcursorResizeAlgo resizeAlgo, List<SCursorImage> images, List<string> overrides, eShapeType shapeType)
        {
            this.directory = directory;
            this.hotspotX = hotspotX;
            this.hotspotY = hotspotY;
            this.resizeAlgo = resizeAlgo;
            this.images = images;
            this.overrides = overrides;
            this.shapeType = shapeType;
        }
    }
    public struct SCursorImage
    {
        public string filename;
        public int size = 0;
        public int delay = 0;

        public SCursorImage()
        {
        }

        public SCursorImage(string filename, int size, int delay)
        {
            this.filename = filename;
            this.size = size;
            this.delay = delay;
        }
    };
    public enum eShapeType
    {
        SHAPE_INVALID = 0,
        SHAPE_PNG,
        SHAPE_SVG,
    };

    public struct XCursorConfigEntry
    {
        public int size = 0;
        public int hotspotX = 0;
        public int hotspotY = 0;
        public int delay = 0;
        public string image;

        public XCursorConfigEntry()
        {
        }

        public XCursorConfigEntry(int size, int hotspotX, int hotspotY, int delay, string image)
        {
            this.size = size;
            this.hotspotX = hotspotX;
            this.hotspotY = hotspotY;
            this.delay = delay;
            this.image = image;
        }
    }


}
