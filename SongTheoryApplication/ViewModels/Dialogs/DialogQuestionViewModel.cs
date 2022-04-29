using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using SongTheoryApplication.Models;
using SongTheoryApplication.Repositories;

namespace SongTheoryApplication.ViewModels.Dialogs;

public class DialogQuestionViewModel
{
    public string QuestionTitle { get; }
    public string QuestionDescription { get; }
    
    public DialogQuestionViewModel(string? questionTitle, string? questionDescription)
    {
        Guard.IsNotNull(questionTitle, nameof(questionTitle));
        Guard.IsNotNull(questionDescription, nameof(questionDescription));

        QuestionTitle = questionTitle;
        QuestionDescription = questionDescription;
    }

}