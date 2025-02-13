using Nevron.Diagram;
using Nevron.Dom;
using Nevron.GraphicsCore;
using Nevron;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;

using System.Drawing;

namespace HQCWeb.FMB_FW
{
    public class Node
    {
        public event Node.RequestNodeRefreshEventHandler RequestNodeRefresh;

        public string Name { get; set; }

        public string Type { get; set; }

        public object Style
        {
            get => this.RawObject is NShape rawObject ? (object)rawObject.Style : (object)null;
            set
            {
                if (!(this.RawObject is NShape rawObject))
                    return;
                NStyle nstyle = (NStyle)rawObject.Style.Clone();
                rawObject.Style = (NStyle)((NAttribute)value).Clone();
                rawObject.Style.TextStyle = nstyle.TextStyle;
                rawObject.Style.InteractivityStyle.Tooltip = nstyle.InteractivityStyle.Tooltip;
                if (this.RequestNodeRefresh != null)
                    this.RequestNodeRefresh(this.Name);
            }
        }

        public object OrgStyle { get; set; }

        public object RawObject { get; set; }

        public override string ToString() => this.Name;

        public bool SetText(string text) => this.SetText(text, Color.Black, eHPosition.Center, eVPosition.Center);

        public bool SetText(string text, Color color) => this.SetText(text, color, eHPosition.Center, eVPosition.Center);

        public bool SetText(string text, Color color, eHPosition hPosition, eVPosition vPosition)
        {
            if (!(this.RawObject is NShape rawObject))
                return false;
            rawObject.Text = text;
            rawObject.Style.TextStyle = new NTextStyle();
            ((NColorFillStyle)rawObject.Style.TextStyle.FillStyle).Color = color;
            rawObject.Style.TextStyle.StringFormatStyle.HorzAlign = (HorzAlign)Enum.Parse(typeof(HorzAlign), hPosition.ToString());
            rawObject.Style.TextStyle.StringFormatStyle.VertAlign = (VertAlign)Enum.Parse(typeof(VertAlign), vPosition.ToString());
            if (this.RequestNodeRefresh != null)
                this.RequestNodeRefresh(this.Name);
            return true;
        }

        public bool SetToolTip(string text)
        {
            if (!(this.RawObject is NShape rawObject))
                return false;
            rawObject.Style.InteractivityStyle.Tooltip.Text = text;
            if (this.RequestNodeRefresh != null)
                this.RequestNodeRefresh(this.Name);
            return true;
        }

        public bool SetColor(Color color)
        {
            if (!(this.RawObject is NShape rawObject))
                return false;
            rawObject.Style.FillStyle = (NFillStyle)new NColorFillStyle(color);
            if (this.RequestNodeRefresh != null)
                this.RequestNodeRefresh(this.Name);
            return true;
        }

        public bool SetColor(HatchStyle style, Color foregroundColor, Color backgroundColor)
        {
            if (!(this.RawObject is NShape rawObject))
                return false;
            rawObject.Style.FillStyle = (NFillStyle)new NHatchFillStyle(style, foregroundColor, backgroundColor);
            if (this.RequestNodeRefresh != null)
                this.RequestNodeRefresh(this.Name);
            return true;
        }

        public bool SetGradient(
          eGradientStyle style,
          eGradientVariant variant,
          Color beginColor,
          Color endColor)
        {
            if (!(this.RawObject is NShape rawObject))
                return false;
            rawObject.Style.FillStyle = (NFillStyle)new NGradientFillStyle((GradientStyle)Enum.Parse(typeof(GradientStyle), style.ToString()), (GradientVariant)Enum.Parse(typeof(GradientVariant), variant.ToString()), beginColor, endColor);
            if (this.RequestNodeRefresh != null)
                this.RequestNodeRefresh(this.Name);
            return true;
        }

        public bool SetLineStyle(Color color, float width = 1f, bool isDashed = false)
        {
            if (!(this.RawObject is NShape rawObject))
                return false;
            rawObject.Style.StrokeStyle = new NStrokeStyle(width, color, isDashed ? LinePattern.Dash : LinePattern.Solid);
            if (this.RequestNodeRefresh != null)
                this.RequestNodeRefresh(this.Name);
            return true;
        }

        public bool SetImage(Bitmap bitmap)
        {
            if (!(this.RawObject is NShape rawObject))
                return false;
            rawObject.Style.FillStyle = (NFillStyle)new NImageFillStyle(bitmap);
            if (this.RequestNodeRefresh != null)
                this.RequestNodeRefresh(this.Name);
            return true;
        }

        public bool ClearStyle()
        {
            if (!(this.RawObject is NShape))
                return false;
            this.Style = this.OrgStyle;
            return true;
        }

        public bool SetStyleRef(object style)
        {
            if (!(this.RawObject is NShape rawObject))
                return false;
            NStyle nstyle = (NStyle)rawObject.Style.Clone();
            rawObject.Style = (NStyle)style;
            rawObject.Style.TextStyle = nstyle.TextStyle;
            rawObject.Style.InteractivityStyle.Tooltip = nstyle.InteractivityStyle.Tooltip;
            if (this.RequestNodeRefresh != null)
                this.RequestNodeRefresh(this.Name);
            return true;
        }

        public delegate void RequestNodeRefreshEventHandler(string name);
    }
}