using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Scopa2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RulesPage : ContentPage
    {
        public RulesPage()
        {
            InitializeComponent();
            rulesLabel.Text = "Scopa is played with a " +
                "standard deck of playing cards with the " +
                "Jacks, Queens and Kings removed leaving " +
                "40 cards to play with.  After shuffling, " +
                "ten of these are distributed at the beginning " +
                "of each round; each player receives three cards " +
                "and four are placed in no-man's-land with their " +
                "faces revealed.\n\nThe players take turns taking cards from no-man's-land " +
                "if the face values of the card they submit from their own hand and the face " +
                "values of the cards that they take from no-man's-land add up to fifteen.  If there is no " +
                "valid move a player discards one of the cards from their hand into no-man's-land.  If " +
                "all cards within no-man's-land are captured at once this is called a scopa.\n\nPlayers recieve " +
                "three more cards from the deck when their first set it used up.\n\nScopas are taken into account " +
                "when awarding bonus points at the end of a round; collecting more sevens than your opponent, " +
                "collecting more diamond-suit cards, collecting the seven of diamonds and collecting the most cards overall " +
                "will each earn a bonus point as well.";
        }

        private async void okayButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}