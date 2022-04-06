using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Scopa2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatsPage : ContentPage
    {
        public static int humanTotalScore = 0;
        public static int aiTotalScore = 0;
        public StatsPage()
        {
            InitializeComponent();
            ShowCurrentTotals();
            AwardDiamondBonuses();
            AwardSevensBonus();
            ScopasAndTotals();
        }
        private void ShowCurrentTotals()
        {
            humanTotalScore = MainPage.humanCaptures.Count;
            aiTotalScore = MainPage.aiCaptures.Count;
            humanResult.Text = MainPage.humanCaptures.Count.ToString();
            aiResult.Text = MainPage.aiCaptures.Count.ToString();
            if (MainPage.humanCaptures.Count > MainPage.aiCaptures.Count)
            {
                humanHasMore.Text = "\u2713";
                aiHasMore.Text = "X";
                humanTotalScore++;
            }
            else if (MainPage.humanCaptures.Count < MainPage.aiCaptures.Count)
            {
                humanHasMore.Text = "X";
                aiHasMore.Text = "\u2713";
                aiTotalScore++;
            }
        }
        private void AwardDiamondBonuses()
        {
            int i = MainPage.humanCaptures.Where(a => a.Suit == Card.SuitType.Diamonds).Count();
            int j = MainPage.aiCaptures.Where(a => a.Suit == Card.SuitType.Diamonds).Count();
            if (i > j)
            {
                humanHasMoreDiamonds.Text = "\u2713";
                aiHasMoreDiamonds.Text = "X";
                humanTotalScore++;
            }
            else if (i < j)
            {
                humanHasMoreDiamonds.Text = "X";
                aiHasMoreDiamonds.Text = "\u2713";
                aiTotalScore++;
            }
            int k = MainPage.humanCaptures.Where(a => (a.Suit == Card.SuitType.Diamonds) && (a.FaceValue == Card.Rank.Seven)).Count();
            int l = MainPage.aiCaptures.Where(a => (a.Suit == Card.SuitType.Diamonds) && (a.FaceValue == Card.Rank.Seven)).Count();
            if (k == 1)
            {
                humanHasSevenOfDiamonds.Text = "\u2713";
                aiHasSevenOfDiamonds.Text = "X";
                humanTotalScore++;
            }
            else if (l == 1)
            {
                humanHasSevenOfDiamonds.Text = "X";
                aiHasSevenOfDiamonds.Text = "\u2713";
                aiTotalScore++;
            }
        }
        private void AwardSevensBonus()
        {
            var humanSevens = from card in MainPage.humanCaptures where card.FaceValue == Card.Rank.Seven select card;
            var aiSevens = from card in MainPage.aiCaptures where card.FaceValue == Card.Rank.Seven select card;
            if (humanSevens.Count() > aiSevens.Count())
            {
                humanHasMoreSevens.Text = "\u2713";
                aiHasMoreSevens.Text = "X";
                humanTotalScore++;
            }
            else if (humanSevens.Count() < aiSevens.Count())
            {
                humanHasMoreSevens.Text = "X";
                aiHasMoreSevens.Text = "\u2713";
                aiTotalScore++;
            }
        }
        private void ScopasAndTotals()
        {
            humanScopasLabel.Text = MainPage.humanScopaCount.ToString();
            aiScopasLabel.Text = MainPage.aiScopaCount.ToString();
            humanTotalScore += MainPage.humanScopaCount;
            aiTotalScore += MainPage.aiScopaCount;
            humanTotalLabel.Text = humanTotalScore.ToString();
            aiTotalLabel.Text = aiTotalScore.ToString();
            if (humanTotalScore > aiTotalScore)
            {
                winnerOrLoser.Text = "W I N N E R";
            }
            else if (humanTotalScore < aiTotalScore)
            {
                winnerOrLoser.Text = "GAME OVER";
            }
            else if (humanTotalScore == aiTotalScore)
            {
                winnerOrLoser.Text = "DEAD HEAT";
            }
        }
        private async void doneButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}