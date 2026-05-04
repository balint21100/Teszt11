namespace Teszt1.Frontend.Edzes;

public partial class ChallengeView : ContentView
{
    public event EventHandler<bool> ChallengeResponded;
    public ChallengeView()
	{
		InitializeComponent();
	}
    private void OnAcceptClicked(object sender, EventArgs e)
    {
        IsVisible = false;
        ChallengeResponded?.Invoke(this, true);
    }

    private void OnCloseClicked(object sender, EventArgs e)
    {
        IsVisible = false;
        ChallengeResponded?.Invoke(this, false);
    }
}