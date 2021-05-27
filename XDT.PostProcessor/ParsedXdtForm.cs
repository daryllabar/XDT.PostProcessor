using System;
using System.Collections.Generic;
using System.Linq;
using Source.DLaB.Common;

namespace XDT.PostProcessor
{
    public class ParsedXdtForm
    {
        public Dictionary<string, List<AttributeInfo>> AttributesByTypeName { get; }
        public Dictionary<string, List<ControlInfo>> ControlsByTypeName { get; }
        public List<AttributeInfo> BooleanAttributes { get; set; }
        public List<AttributeInfo> DateAttributes { get; set; }
        public List<AttributeInfo> NumberAttributes { get; set; }
        public List<AttributeInfo> LookupAttributes { get; set; }
        public List<AttributeInfo> OptionSetAttributes { get; set; }
        public List<AttributeInfo> MultiSelectAttributes { get; set; }
        public List<ControlInfo> AttributeControls { get; set; }
        public List<ControlInfo> BaseControls { get; set; }
        public List<ControlInfo> DateControls { get; set; }
        public List<ControlInfo> IFrameControls { get; set; }
        public List<ControlInfo> KbSearchControls { get; set; }
        public List<ControlInfo> LookupControls { get; set; }
        public List<ControlInfo> NoteControls { get; set; }
        public List<ControlInfo> MultiSelectControls { get; set; }
        public List<ControlInfo> NumberControls { get; set; }
        public List<ControlInfo> StringControls { get; set; }
        public List<ControlInfo> SubgridControls { get; set; }
        public List<ControlInfo> TimerControls { get; set; }
        public List<ControlInfo> WebResourceControls { get; set; }

        public ParsedXdtForm()
        {
            AttributesByTypeName = new Dictionary<string, List<AttributeInfo>>();
            ControlsByTypeName = new Dictionary<string, List<ControlInfo>>();
            BooleanAttributes = new List<AttributeInfo>();
            DateAttributes = new List<AttributeInfo>();
            NumberAttributes = new List<AttributeInfo>();
            LookupAttributes = new List<AttributeInfo>();
            OptionSetAttributes = new List<AttributeInfo>();
            MultiSelectAttributes = new List<AttributeInfo>();
            AttributeControls = new List<ControlInfo>();
            BaseControls = new List<ControlInfo>();
            DateControls = new List<ControlInfo>();
            IFrameControls = new List<ControlInfo>();
            KbSearchControls = new List<ControlInfo>();
            LookupControls = new List<ControlInfo>();
            NoteControls = new List<ControlInfo>();
            MultiSelectControls = new List<ControlInfo>();
            NumberControls = new List<ControlInfo>();
            StringControls = new List<ControlInfo>();
            SubgridControls = new List<ControlInfo>();
            TimerControls = new List<ControlInfo>();
            WebResourceControls = new List<ControlInfo>();
        }
    }
}
