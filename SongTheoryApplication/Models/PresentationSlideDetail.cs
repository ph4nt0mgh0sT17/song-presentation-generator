using System.Collections.Generic;
using System.Windows.Documents;

namespace SongTheoryApplication.Models;

public class PresentationSlideDetail
{
    public List<PresentationTextStyle> PresentationTextStyles { get; }
    public PresentationFormatStyle PresentationFormatStyle { get; set; }
    

    public PresentationSlideDetail(PresentationFormatStyle presentationFormatStyle)
    {
        PresentationFormatStyle = presentationFormatStyle;
        PresentationTextStyles = new List<PresentationTextStyle>();
    }
}