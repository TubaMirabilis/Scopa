using MoreLinq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Scopa2
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public static List<Card> humanCaptures = new List<Card>();
        public static List<Card> aiCaptures = new List<Card>();
        public static int humanScopaCount = 0;
        public static int aiScopaCount = 0;
        private Rectangle deckPosition = new Rectangle(0.500, 0.500, 0.080, 0.240);
        private Image deckImage;
        private Label humScoreLabel;
        private Label aiScoreLabel;
        private Button nextRoundButton;
        private List<ImageButton> tableImgs = new List<ImageButton>();  
        private Card card1;
        private Card card2;
        private Card card3;
        private Card card4;
        private Card card5; 
        private Card card6;
        private Card card7;
        private Card card8;
        private Card card9;
        private Card card10;
        private List<Card> tableCards = new List<Card>();
        private Card humanCard1;
        private Card humanCard2;
        private Card humanCard3;
        private List<Card> humanCards;
        private Card aiCard1;
        private Card aiCard2;
        private Card aiCard3;
        private List<Card> aiCards;
        private List<ImageButton> aiImgs;
        private ImageButton humanImg1; 
        private ImageButton humanImg2; 
        private ImageButton humanImg3;
        private ImageButton tableCard1 = new ImageButton();
        private ImageButton tableCard2 = new ImageButton();
        private ImageButton tableCard3 = new ImageButton();
        private ImageButton tableCard4 = new ImageButton();
        private ImageButton tableCard5 = new ImageButton();
        private ImageButton tableCard6 = new ImageButton();
        private ImageButton tableCard7 = new ImageButton();
        private ImageButton tableCard8 = new ImageButton();
        private ImageButton tableCard9 = new ImageButton();
        private ImageButton tableCard10 = new ImageButton();
        private bool human1Pressed = false;
        private bool human2Pressed = false;
        private bool human3Pressed = false;
        private bool table1Pressed = false;
        private bool table2Pressed = false;
        private bool table3Pressed = false;
        private bool table4Pressed = false;
        private bool table5Pressed = false;
        private bool table6Pressed = false;
        private bool table7Pressed = false;
        private bool table8Pressed = false;
        private bool table9Pressed = false;
        private bool table10Pressed = false;
        private List<ImageButton> humanImgs;
        private List<IList<int>> aiValidCombos = new List<IList<int>>();
        private IList<int> bestMove;
        private int captureCount;

        public MainPage()
        {
            InitializeComponent();
        }
        private async void showRulesButton_Clicked(object sender, EventArgs e)
        {
            RulesPage rulesPage = new RulesPage();
            await Navigation.PushModalAsync(rulesPage);
        }
        //Prepare the event for the "TAP TO PLAY" button:
        private async void playButton_Clicked(object sender, EventArgs e)
        {
            //Create a deck of cards and shuffle it a few times:
            Deck.CreateDeck();
            Deck.Shuffle(3);
            //Get the 2D representation of the top of the deck prepared:
            deckImage = new Image
            {
                Source = ImageSource.FromResource("Scopa2.images.purple_back.jpg",
                typeof(ImageResourceExtension).GetTypeInfo().Assembly),
                Aspect = Aspect.Fill,
                Opacity = 0
            };
            Rectangle rect1 = new Rectangle(0.035, 0.020, 0.185, 0.240);
            Rectangle rect2 = new Rectangle(0.965, 0.020, 0.185, 0.240);
            StackLayout humanScoreStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    new Label {Text = "Your\nScore:", TextColor = Color.GreenYellow,
                        FontSize = 20,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Center},
                }
            };
            StackLayout aiScoreStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    new Label {Text = "AI\nScore:", TextColor = Color.GreenYellow,
                        FontSize = 20,
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.Center},
                }
            };
            humScoreLabel = new Label
            {
                Text = humanCaptures.Count.ToString(),
                TextColor = Color.GreenYellow,
                FontSize = 40,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
            aiScoreLabel = new Label
            {
                Text = aiCaptures.Count.ToString(),
                TextColor = Color.GreenYellow,
                FontSize = 40,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
            AbsoluteLayout.SetLayoutBounds(deckImage, deckPosition);
            AbsoluteLayout.SetLayoutFlags(deckImage, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(humanScoreStack, rect1);
            AbsoluteLayout.SetLayoutFlags(humanScoreStack, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(aiScoreStack, rect2);
            AbsoluteLayout.SetLayoutFlags(aiScoreStack, AbsoluteLayoutFlags.All);
            //Fade out the home screen:
            await Task.WhenAll(whiteBorder.FadeTo(0, 444), greenStack.FadeTo(0, 666));
            game.Children.Remove(whiteBorder);
            game.Children.Remove(greenStack);
            //Show the image representing the deck and four images representing
            //the four cards that the dealer places upon the table after shuffling;
            //these cards will eventually be displayed face-up:
            game.Children.Add(deckImage);
            humanScoreStack.Children.Add(humScoreLabel);
            aiScoreStack.Children.Add(aiScoreLabel);
            game.Children.Add(humanScoreStack);
            game.Children.Add(aiScoreStack);
            await deckImage.FadeTo(1, 500);
            await Task.WhenAll(PositionCards(), NewSubDeal());
        }
        private async void NextRound_Clicked(object sender, EventArgs e)
        {
            humanImg1.Clicked -= OnHumanImg1Clicked;
            humanImg2.Clicked -= OnHumanImg2Clicked;
            humanImg3.Clicked -= OnHumanImg3Clicked;
            tableCard1.Clicked -= OnTableImg1Clicked;
            tableCard2.Clicked -= OnTableImg2Clicked;
            tableCard3.Clicked -= OnTableImg3Clicked;
            tableCard4.Clicked -= OnTableImg4Clicked;
            tableCard5.Clicked -= OnTableImg5Clicked;
            tableCard6.Clicked -= OnTableImg6Clicked;
            tableCard7.Clicked -= OnTableImg7Clicked;
            tableCard8.Clicked -= OnTableImg8Clicked;
            tableCard9.Clicked -= OnTableImg9Clicked;
            tableCard10.Clicked -= OnTableImg10Clicked;
            humanCaptures.Clear();
            aiCaptures.Clear();
            tableCards.Clear();
            humScoreLabel.Text = humanCaptures.Count.ToString();
            aiScoreLabel.Text = aiCaptures.Count.ToString();
            humanScopaCount = 0;
            aiScopaCount = 0;
            await nextRoundButton.FadeTo(0, 500);
            game.Children.Remove(nextRoundButton);
            nextRoundButton.Clicked -= NextRound_Clicked;
            Deck.CreateDeck();
            Deck.Shuffle(3);
            int x = Deck.cards.Count;
            AbsoluteLayout.SetLayoutBounds(deckImage, deckPosition);
            AbsoluteLayout.SetLayoutFlags(deckImage, AbsoluteLayoutFlags.All);
            game.Children.Add(deckImage);
            await deckImage.FadeTo(1, 500);
            await Task.WhenAll(PositionCards(), NewSubDeal());
        }
        //Prepare the images that represent the four cards that the dealer places upon
        //the table after shuffling; this method also makes the previously invisible images
        //visible and situates each one to an appropriate place within the layout through animation:
        private async Task PositionCards()
        {
            //Four Card objects for the 'table cards':
            card1 = Deck.cards[0];
            card2 = Deck.cards[1];
            card3 = Deck.cards[2];
            card4 = Deck.cards[3];
            tableCards.Add(card1);
            tableCards.Add(card2);
            tableCards.Add(card3);
            tableCards.Add(card4);
            //Shrink the deck to 36 cards:
            Deck.cards.RemoveRange(0, 4);
            //Some image objects:
            tableCard1.Source = CardID(card1);
            tableCard2.Source = CardID(card2);
            tableCard3.Source = CardID(card3);
            tableCard4.Source = CardID(card4);
            tableCard1.Clicked += OnTableImg1Clicked;
            tableCard2.Clicked += OnTableImg2Clicked;
            tableCard3.Clicked += OnTableImg3Clicked;
            tableCard4.Clicked += OnTableImg4Clicked;
            tableImgs.Add(tableCard1);
            tableImgs.Add(tableCard2);
            tableImgs.Add(tableCard3);
            tableImgs.Add(tableCard4);
            //Assign the arrayed images' properties, add the images to the layout and fade in:
            foreach (ImageButton i in tableImgs)
            {
                i.IsEnabled = false;
                i.Aspect = Aspect.Fill;
                i.Opacity = 0;
                i.CornerRadius = 10;
                AbsoluteLayout.SetLayoutBounds(i, deckPosition);
                AbsoluteLayout.SetLayoutFlags(i, AbsoluteLayoutFlags.All);
                game.Children.Add(i);
                Task.WhenAll(i.FadeTo(1, 500));
            }
            //Place the cards at conventient non-overlapping locations:
            await Task.WhenAll
                (tableCard1.TranslateTo(-1.1 * tableCard1.Width, (-0.525 * tableCard1.Height), 500),
                 tableCard2.TranslateTo(1.1 * tableCard2.Width, (-0.525 * tableCard2.Height), 500),
                 tableCard3.TranslateTo(-1.1 * tableCard3.Width, 0.525 * tableCard3.Height, 500),
                 tableCard4.TranslateTo(1.1 * tableCard4.Width, 0.525 * tableCard4.Height, 500));
        }
        private async Task NewSubDeal()
        {
            humanCard1 = Deck.cards[0];
            humanCard2 = Deck.cards[1];
            humanCard3 = Deck.cards[2];
            humanCards = new List<Card> { humanCard1, humanCard2, humanCard3 };
            aiCard1 = Deck.cards[3];
            aiCard2 = Deck.cards[4];
            aiCard3 = Deck.cards[5];
            aiCards = new List<Card> { aiCard1, aiCard2, aiCard3 };
            humanImg1 = new ImageButton();
            humanImg2 = new ImageButton();
            humanImg3 = new ImageButton();
            humanImg1.Source = CardID(humanCard1);
            humanImg2.Source = CardID(humanCard2);
            humanImg3.Source = CardID(humanCard3);
            humanImg1.Clicked += OnHumanImg1Clicked;
            humanImg2.Clicked += OnHumanImg2Clicked;
            humanImg3.Clicked += OnHumanImg3Clicked;
            humanImgs = new List<ImageButton> { humanImg1, humanImg2, humanImg3 };
            ImageButton aiImg1 = new ImageButton();
            ImageButton aiImg2 = new ImageButton();
            ImageButton aiImg3 = new ImageButton();
            aiImgs = new List<ImageButton> { aiImg1, aiImg2, aiImg3 };
            foreach (ImageButton i in humanImgs)
            {
                i.IsEnabled = true;
                i.Aspect = Aspect.Fill;
                i.Opacity = 0;
                i.CornerRadius = 10;
                AbsoluteLayout.SetLayoutBounds(i, deckPosition);
                AbsoluteLayout.SetLayoutFlags(i, AbsoluteLayoutFlags.All);
                game.Children.Add(i);
                Task.WhenAll(i.FadeTo(1, 500));
            }
            foreach (ImageButton i in aiImgs)
            {
                i.IsEnabled = false;
                i.Source = ImageSource.FromResource("Scopa2.images.purple_back.jpg",
                typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                i.Aspect = Aspect.Fill;
                i.Opacity = 0;
                i.CornerRadius = 10;
                AbsoluteLayout.SetLayoutBounds(i, deckPosition);
                AbsoluteLayout.SetLayoutFlags(i, AbsoluteLayoutFlags.All);
                game.Children.Add(i);
                Task.WhenAll(i.FadeTo(1, 500));
                tableCard10.Opacity = 1;
            }
            await Task.WhenAll
                (humanImg1.TranslateTo((-1.1 * humanImg1.Width), (1.565 * humanImg1.Height), 500),
                 humanImg2.TranslateTo(0, (1.565 * humanImg2.Height), 500),
                 humanImg3.TranslateTo((1.1 * humanImg3.Width), (1.565 * humanImg3.Height), 500));
            await Task.WhenAll
                 (aiImg1.TranslateTo((-1.1 * aiImg1.Width), (-1.565 * aiImg1.Height), 500),
                 aiImg2.TranslateTo(0, (-1.565 * aiImg2.Height), 500),
                 aiImg3.TranslateTo((1.1 * aiImg3.Width), (-1.565 * aiImg3.Height), 500));
            Deck.cards.RemoveRange(0, 6);
        }
        private async void OnHumanImg1Clicked(object sender, EventArgs e)
        {
            human1Pressed = true;
            humanImg2.IsEnabled = false;
            humanImg3.IsEnabled = false;
            await AccessTableImgs(humanCard1, humanImg1);
        }
        private async void OnHumanImg2Clicked(object sender, EventArgs e)
        {
            human2Pressed = true;
            humanImg1.IsEnabled = false;
            humanImg3.IsEnabled = false;
            await AccessTableImgs(humanCard2, humanImg2);
        }
        private async void OnHumanImg3Clicked(object sender, EventArgs e)
        {
            human3Pressed = true;
            humanImg1.IsEnabled = false;
            humanImg2.IsEnabled = false;
            await AccessTableImgs(humanCard3, humanImg3);
        }
        private async void OnTableImg1Clicked(object sender, EventArgs e)
        {
            table1Pressed = true;
            await AdjudicateCapture(tableCard1, card1);
        }
        private async void OnTableImg2Clicked(object sender, EventArgs e)
        {
            table2Pressed = true;
            await AdjudicateCapture(tableCard2, card2);
        }

        private async void OnTableImg3Clicked(object sender, EventArgs e)
        {
            table3Pressed = true;
            await AdjudicateCapture(tableCard3, card3);
        }

        private async void OnTableImg4Clicked(object sender, EventArgs e)
        {
            table4Pressed = true;
            await AdjudicateCapture(tableCard4, card4);
        }
        private async void OnTableImg5Clicked(object sender, EventArgs e)
        {
            table5Pressed = true;
            await AdjudicateCapture(tableCard5, card5);
        }
        private async void OnTableImg6Clicked(object sender, EventArgs e)
        {
            table6Pressed = true;
            await AdjudicateCapture(tableCard6, card6);
        }
        private async void OnTableImg7Clicked(object sender, EventArgs e)
        {
            table7Pressed = true;
            await AdjudicateCapture(tableCard7, card7);
        }
        private async void OnTableImg8Clicked(object sender, EventArgs e)
        {
            table8Pressed = true;
            await AdjudicateCapture(tableCard8, card8);
        }
        private async void OnTableImg9Clicked(object sender, EventArgs e)
        {
            table9Pressed = true;
            await AdjudicateCapture(tableCard9, card9);
        }
        private async void OnTableImg10Clicked(object sender, EventArgs e)
        {
            table10Pressed = true;
            await AdjudicateCapture(tableCard10, card10);
        }
        private async Task AccessTableImgs(Card i, ImageButton j)
        {
            GetCombos(out int x);
            if (x == 0)
            {
                if (human1Pressed == true)
                {
                    human1Pressed = false;
                    j.Clicked -= OnHumanImg1Clicked;
                }
                else if (human2Pressed == true)
                {
                    human2Pressed = false;
                    j.Clicked -= OnHumanImg2Clicked;
                }
                else if (human3Pressed == true)
                {
                    human3Pressed = false;
                    j.Clicked -= OnHumanImg3Clicked;
                }
                await DiscardCards(i, j);
                j.IsEnabled = false;
                humanCards.Remove(i);
                humanImgs.Remove(j);
                captureCount = 0;
                await AiTurn();
            }
            else if (x > 0)
            {
                if (StopNastyBug(i))
                {
                    captureCount += (int)i.FaceValue;
                    j.BorderWidth = 2.5;
                    j.BorderColor = Color.GreenYellow;
                    foreach (ImageButton k in humanImgs)
                    {
                        k.IsEnabled = false;
                    }
                    foreach (ImageButton l in tableImgs)
                    {
                        l.IsEnabled = true;
                    }
                }
                else if (!StopNastyBug(i))
                {
                    foreach (ImageButton k in humanImgs)
                    {
                        k.IsEnabled = true;
                    }
                    j.IsEnabled = false;
                }
            }
        }
        private void GetCombos(out int number)
        {
            List<IList<int>> validCombos = new List<IList<int>>();
            List<int> numbers = new List<int>();
            for (int i = 0; i < tableCards.Count; i++)
            {
                numbers.Add((int)tableCards[i].FaceValue);
            }
            List<IList<int>> validCombos1 = numbers.Subsets().Where(a => a.Sum() == (15 - (int)humanCard1.FaceValue)).ToList();
            List<IList<int>> validCombos2 = numbers.Subsets().Where(a => a.Sum() == (15 - (int)humanCard2.FaceValue)).ToList();
            List<IList<int>> validCombos3 = numbers.Subsets().Where(a => a.Sum() == (15 - (int)humanCard3.FaceValue)).ToList();
            if (humanCards.Contains(humanCard1))
            {
                validCombos.AddRange(validCombos1);
            }
            if (humanCards.Contains(humanCard2))
            {
                validCombos.AddRange(validCombos2);
            }
            if (humanCards.Contains(humanCard3))
            {
                validCombos.AddRange(validCombos3);
            }
            number = validCombos.Count;
        }
        private bool StopNastyBug(Card i)
        {
            List<int> numbers = new List<int>();
            for (int x = 0; x < tableCards.Count; x++)
            {
                numbers.Add((int)tableCards[x].FaceValue);
            }
            if ((numbers.Sum() + (int)i.FaceValue) < 15)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private async Task AdjudicateCapture(ImageButton i, Card j)
        {
            captureCount += (int)j.FaceValue;
            if (captureCount < 15)
            {
                i.BorderWidth = 2.5;
                i.BorderColor = Color.GreenYellow;
                i.IsEnabled = false;
            }
            else if (captureCount > 15)
            {
                captureCount = 0;
                foreach (ImageButton k in tableImgs)
                {
                    k.BorderWidth = 0;
                    k.IsEnabled = false;
                }
                foreach (ImageButton l in humanImgs)
                {
                    l.IsEnabled = true;
                    l.BorderWidth = 0;
                }
                human1Pressed = false;
                human2Pressed = false;
                human3Pressed = false;
                table1Pressed = false;
                table2Pressed = false;
                table3Pressed = false;
                table4Pressed = false;
                table5Pressed = false;
                table6Pressed = false;
                table7Pressed = false;
                table8Pressed = false;
                table9Pressed = false;
                table10Pressed = false;
            }
            else if (captureCount == 15)
            {
                captureCount = 0;
                foreach (ImageButton k in tableImgs)
                {
                    k.IsEnabled = false;
                    k.BorderWidth = 0;
                }
                foreach (ImageButton l in humanImgs)
                {
                    l.IsEnabled = false;
                    l.BorderWidth = 0;
                }
                if (human1Pressed == true)
                {
                    humanImgs.Remove(humanImg1);
                    game.Children.Remove(humanImg1);
                    humanCaptures.Add(humanCard3);
                    humanCards.Remove(humanCard1);
                    human1Pressed = false;
                }
                else if (human2Pressed == true)
                {
                    humanImgs.Remove(humanImg2);
                    game.Children.Remove(humanImg2);
                    humanCaptures.Add(humanCard2);
                    humanCards.Remove(humanCard2);
                    human2Pressed = false;
                }
                else if (human3Pressed == true)
                {
                    humanImgs.Remove(humanImg3);
                    game.Children.Remove(humanImg3);
                    humanCaptures.Add(humanCard3);
                    humanCards.Remove(humanCard3);
                    human3Pressed = false;
                }
                if (table1Pressed == true)
                {
                    tableImgs.Remove(tableCard1);
                    game.Children.Remove(tableCard1);
                    tableCards.Remove(card1);
                    humanCaptures.Add(card1);
                    table1Pressed = false;
                }
                if (table2Pressed == true)
                {
                    tableImgs.Remove(tableCard2);
                    game.Children.Remove(tableCard2);
                    tableCards.Remove(card2);
                    humanCaptures.Add(card2);
                    table2Pressed = false;
                }
                if (table3Pressed == true)
                {
                    tableImgs.Remove(tableCard3);
                    game.Children.Remove(tableCard3);
                    tableCards.Remove(card3);
                    humanCaptures.Add(card3);
                    table3Pressed = false;
                }
                if (table4Pressed == true)
                {
                    tableImgs.Remove(tableCard4);
                    game.Children.Remove(tableCard4);
                    tableCards.Remove(card4);
                    humanCaptures.Add(card4);
                    table4Pressed = false;
                }
                if (table5Pressed == true)
                {
                    tableImgs.Remove(tableCard5);
                    game.Children.Remove(tableCard5);
                    tableCards.Remove(card5);
                    humanCaptures.Add(card5);
                    table5Pressed = false;
                }
                if (table6Pressed == true)
                {
                    tableImgs.Remove(tableCard6);
                    game.Children.Remove(tableCard6);
                    tableCards.Remove(card6);
                    humanCaptures.Add(card6);
                    table6Pressed = false;
                }
                if (table7Pressed == true)
                {
                    tableImgs.Remove(tableCard7);
                    game.Children.Remove(tableCard7);
                    tableCards.Remove(card7);
                    humanCaptures.Add(card7);
                    table7Pressed = false;
                }
                if (table8Pressed == true)
                {
                    tableImgs.Remove(tableCard8);
                    game.Children.Remove(tableCard8);
                    tableCards.Remove(card8);
                    humanCaptures.Add(card8);
                    table8Pressed = false;
                }
                if (table9Pressed == true)
                {
                    tableImgs.Remove(tableCard9);
                    game.Children.Remove(tableCard9);
                    tableCards.Remove(card9);
                    humanCaptures.Add(card9);
                    table9Pressed = false;
                }
                if (table10Pressed == true)
                {
                    tableImgs.Remove(tableCard10);
                    game.Children.Remove(tableCard10);
                    tableCards.Remove(card10);
                    humanCaptures.Add(card10);
                    table10Pressed = false;
                }
                if (tableCards.Count == 0)
                {
                    ScopaPopup scoPop = new ScopaPopup();
                    await PopupNavigation.PushAsync(scoPop);
                    await Task.Delay(500);
                    await PopupNavigation.PopAllAsync();
                    humanScopaCount++;
                }
                humScoreLabel.Text = humanCaptures.Count.ToString();
                await AiTurn();
            }
        }
        private async Task AiTurn()
        {
            var rand = new Random();
            int num = rand.Next(aiImgs.Count);
            GetAiCombos(out int x);
            if (x == 0)
            {
                aiImgs[num].Source = CardID(aiCards[num]);
                await DiscardCards(aiCards[num], aiImgs[num]);
                aiCards.Remove(aiCards[num]);
                aiImgs.Remove(aiImgs[num]);
                captureCount = 0;
            }
            else if (x > 0)
            {
                aiImgs[num].BorderColor = Color.GreenYellow;
                aiImgs[num].BorderWidth = 2.5;
                SetAiTargets(aiValidCombos, out Card tempCard);
                aiImgs[num].Source = CardID(tempCard);
                await AiCapture();
                game.Children.Remove(aiImgs[num]);
                aiImgs.Remove(aiImgs[num]);
                if (aiCard1 == tempCard)
                {
                    aiCaptures.Add(aiCard1);
                    aiCards.Remove(aiCard1);
                }
                else if (aiCard2 == tempCard)
                {
                    aiCaptures.Add(aiCard2);
                    aiCards.Remove(aiCard2);
                }
                else if (aiCard3 == tempCard)
                {
                    aiCaptures.Add(aiCard3);
                    aiCards.Remove(aiCard3);
                }
                aiValidCombos.Clear();
                table1Pressed = false;
                table2Pressed = false;
                table3Pressed = false;
                table4Pressed = false;
                table5Pressed = false;
                table6Pressed = false;
                table7Pressed = false;
                table8Pressed = false;
                table9Pressed = false;
                table10Pressed = false;
                aiScoreLabel.Text = aiCaptures.Count.ToString();
            }
            foreach (ImageButton i in humanImgs)
            {
                i.IsEnabled = true;
            }
            if (Deck.cards.Count >= 12 && aiCards.Count == 0)
            {
                humanImg1.Clicked -= OnHumanImg1Clicked;
                humanImg2.Clicked -= OnHumanImg2Clicked;
                humanImg3.Clicked -= OnHumanImg3Clicked;
                await NewSubDeal();
            }
            else if (Deck.cards.Count == 6 && aiCards.Count == 0)
            {
                humanImg1.Clicked -= OnHumanImg1Clicked;
                humanImg2.Clicked -= OnHumanImg2Clicked;
                humanImg3.Clicked -= OnHumanImg3Clicked;
                deckImage.Opacity = 0;
                game.Children.Remove(deckImage);
                await NewSubDeal();
            }
            else if (Deck.cards.Count == 0 && aiCards.Count == 0)
            {
                await EndRound();
            }
        }
        private void GetAiCombos(out int number)
        {
            List<int> numbers = new List<int>();
            for (int i = 0; i < tableCards.Count; i++)
            {
                numbers.Add((int)tableCards[i].FaceValue);
            }
            List<IList<int>> aiValidCombos1 = numbers.Subsets().Where(a => a.Sum() == (15 - (int)aiCard1.FaceValue)).ToList();
            List<IList<int>> aiValidCombos2 = numbers.Subsets().Where(a => a.Sum() == (15 - (int)aiCard2.FaceValue)).ToList();
            List<IList<int>> aiValidCombos3 = numbers.Subsets().Where(a => a.Sum() == (15 - (int)aiCard3.FaceValue)).ToList();
            if (aiCards.Contains(aiCard1))
            {
                aiValidCombos.AddRange(aiValidCombos1);
            }
            if (aiCards.Contains(aiCard2))
            {
                aiValidCombos.AddRange(aiValidCombos2);
            }
            if (aiCards.Contains(aiCard3))
            {
                aiValidCombos.AddRange(aiValidCombos3);
            }
            number = aiValidCombos.Count;
        }
        private void SetAiTargets(List<IList<int>> allCombos, out Card lassoCard)
        {
            IOrderedEnumerable<IList<int>> orderedMoves = from move in allCombos
                                                          orderby move.Count descending
                                                          select move;
            bestMove = orderedMoves.FirstOrDefault();
            int targetSum = bestMove.Sum();
            int lassoCardValue = 15 - targetSum;
            IEnumerable<Card> lassoCards = aiCards.Where(a => lassoCardValue == (int)a.FaceValue);
            lassoCard = lassoCards.FirstOrDefault();
        }
        private async Task AiCapture()
        {
            foreach (int num in bestMove)
            {
                if (tableCards.Contains(card1) && (int)card1.FaceValue == num && table1Pressed == false)
                {
                    table1Pressed = true;
                    tableCard1.BorderColor = Color.GreenYellow;
                    tableCard1.BorderWidth = 2.5;
                    continue;
                }
                else if (tableCards.Contains(card2) && (int)card2.FaceValue == num && table2Pressed == false)
                {
                    table2Pressed = true;
                    tableCard2.BorderColor = Color.GreenYellow;
                    tableCard2.BorderWidth = 2.5;
                    continue;
                }
                else if (tableCards.Contains(card3) && (int)card3.FaceValue == num && table3Pressed == false)
                {
                    table3Pressed = true;
                    tableCard3.BorderColor = Color.GreenYellow;
                    tableCard3.BorderWidth = 2.5;
                    continue;
                }
                else if (tableCards.Contains(card4) && (int)card4.FaceValue == num && table4Pressed == false)
                {
                    table4Pressed = true;
                    tableCard4.BorderColor = Color.GreenYellow;
                    tableCard4.BorderWidth = 2.5;
                    continue;
                }
                else if (tableCards.Contains(card5) && (int)card5.FaceValue == num && table5Pressed == false)
                {
                    table5Pressed = true;
                    tableCard5.BorderColor = Color.GreenYellow;
                    tableCard5.BorderWidth = 2.5;
                    continue;
                }
                else if (tableCards.Contains(card6) && (int)card6.FaceValue == num && table6Pressed == false)
                {
                    table6Pressed = true;
                    tableCard6.BorderColor = Color.GreenYellow;
                    tableCard6.BorderWidth = 2.5;
                    continue;
                }
                else if (tableCards.Contains(card7) && (int)card7.FaceValue == num && table7Pressed == false)
                {
                    table7Pressed = true;
                    tableCard7.BorderColor = Color.GreenYellow;
                    tableCard7.BorderWidth = 2.5;
                    continue;
                }
                else if (tableCards.Contains(card8) && (int)card8.FaceValue == num && table8Pressed == false)
                {
                    table8Pressed = true;
                    tableCard8.BorderColor = Color.GreenYellow;
                    tableCard8.BorderWidth = 2.5;
                    continue;
                }
                else if (tableCards.Contains(card9) && (int)card9.FaceValue == num && table9Pressed == false)
                {
                    table9Pressed = true;
                    tableCard9.BorderColor = Color.GreenYellow;
                    tableCard9.BorderWidth = 2.5;
                    continue;
                }
                else if (tableCards.Contains(card10) && (int)card10.FaceValue == num && table10Pressed == false)
                {
                    table10Pressed = true;
                    tableCard10.BorderColor = Color.GreenYellow;
                    tableCard10.BorderWidth = 2.5;
                    continue;
                }
            }
            await Task.Delay(250);
            if (table1Pressed == true)
            {
                await tableCard1.ScaleTo(2.0, 333);
                await tableCard1.ScaleTo(0.0, 667);
                tableCard1.BorderWidth = 0;
                aiCaptures.Add(card1);
                tableCards.Remove(card1);
                tableImgs.Remove(tableCard1);
                game.Children.Remove(tableCard1);
                await tableCard1.ScaleTo(1.0, 0);
            }
            if (table2Pressed == true)
            {
                await tableCard2.ScaleTo(2.0, 333);
                await tableCard2.ScaleTo(0.0, 667);
                tableCard2.BorderWidth = 0;
                aiCaptures.Add(card2);
                tableCards.Remove(card2);
                tableImgs.Remove(tableCard2);
                game.Children.Remove(tableCard2);
                await tableCard2.ScaleTo(1.0, 0);
            }
            if (table3Pressed == true)
            {
                await tableCard3.ScaleTo(2.0, 333);
                await tableCard3.ScaleTo(0.0, 667);
                tableCard3.BorderWidth = 0;
                aiCaptures.Add(card3);
                tableCards.Remove(card3);
                tableImgs.Remove(tableCard3);
                game.Children.Remove(tableCard3);
                await tableCard3.ScaleTo(1.0, 0);
            }
            if (table4Pressed == true)
            {
                await tableCard4.ScaleTo(2.0, 333);
                await tableCard4.ScaleTo(0.0, 667);
                tableCard4.BorderWidth = 0;
                aiCaptures.Add(card4);
                tableCards.Remove(card4);
                tableImgs.Remove(tableCard4);
                game.Children.Remove(tableCard4);
                await tableCard4.ScaleTo(1.0, 0);
            }
            if (table5Pressed == true)
            {
                await tableCard5.ScaleTo(2.0, 333);
                await tableCard5.ScaleTo(0.0, 667);
                tableCard5.BorderWidth = 0;
                aiCaptures.Add(card5);
                tableCards.Remove(card5);
                tableImgs.Remove(tableCard5);
                game.Children.Remove(tableCard5);
                await tableCard5.ScaleTo(1.0, 0);
            }
            if (table6Pressed == true)
            {
                await tableCard6.ScaleTo(2.0, 333);
                await tableCard6.ScaleTo(0.0, 667);
                tableCard6.BorderWidth = 0;
                aiCaptures.Add(card6);
                tableCards.Remove(card6);
                tableImgs.Remove(tableCard6);
                game.Children.Remove(tableCard6);
                await tableCard6.ScaleTo(1.0, 0);
            }
            if (table7Pressed == true)
            {
                await tableCard7.ScaleTo(2.0, 333);
                await tableCard7.ScaleTo(0.0, 667);
                tableCard7.BorderWidth = 0;
                aiCaptures.Add(card7);
                tableCards.Remove(card7);
                tableImgs.Remove(tableCard7);
                game.Children.Remove(tableCard7);
                await tableCard7.ScaleTo(1.0, 0);
            }
            if (table8Pressed == true)
            {
                await tableCard8.ScaleTo(2.0, 333);
                await tableCard8.ScaleTo(0.0, 667);
                tableCard8.BorderWidth = 0;
                aiCaptures.Add(card8);
                tableCards.Remove(card8);
                tableImgs.Remove(tableCard8);
                game.Children.Remove(tableCard8);
                await tableCard8.ScaleTo(1.0, 0);
            }
            if (table9Pressed == true)
            {
                await tableCard9.ScaleTo(2.0, 333);
                await tableCard9.ScaleTo(0.0, 667);
                tableCard9.BorderWidth = 0;
                aiCaptures.Add(card9);
                tableCards.Remove(card9);
                tableImgs.Remove(tableCard9);
                game.Children.Remove(tableCard9);
                await tableCard9.ScaleTo(1.0, 0);
            }
            if (table10Pressed == true)
            {
                await tableCard10.ScaleTo(2.0, 333);
                await tableCard10.ScaleTo(0.0, 667);
                tableCard10.BorderWidth = 0;
                aiCaptures.Add(card10);
                tableCards.Remove(card10);
                tableImgs.Remove(tableCard10);
                game.Children.Remove(tableCard10);
                await tableCard10.ScaleTo(1.0, 0);
            }
            if (tableCards.Count == 0)
            {
                ScopaPopup scoPop = new ScopaPopup();
                await PopupNavigation.PushAsync(scoPop);
                await Task.Delay(500);
                await PopupNavigation.PopAllAsync();
                aiScopaCount++;
            }
            aiScoreLabel.Text = aiCaptures.Count.ToString();
        }
        private async Task DiscardCards(Card i, ImageButton j)
        {
            if (!tableImgs.Contains(tableCard5))
            {
                tableCard5 = j;
                j.Clicked += OnTableImg5Clicked;
                j.Opacity = 1;
                tableImgs.Add(tableCard5);
                card5 = i;
                tableCards.Add(card5);
                await Task.WhenAll(tableCard5.TranslateTo(-2.2 * tableCard5.Width, -0.525 * tableCard5.Height, 500));
            }
            else if (!tableImgs.Contains(tableCard6))
            {
                tableCard6 = j;
                j.Clicked += OnTableImg6Clicked;
                j.Opacity = 1;
                tableImgs.Add(tableCard6);
                card6 = new Card(i.FaceValue, i.Suit);
                tableCards.Add(card6);
                await Task.WhenAll(tableCard6.TranslateTo(2.2 * tableCard6.Width, 0.525 * tableCard6.Height, 500));
            }
            else if (!tableImgs.Contains(tableCard7))
            {
                tableCard7 = j;
                j.Clicked += OnTableImg7Clicked;
                j.Opacity = 1;
                tableImgs.Add(tableCard7);
                card7 = new Card(i.FaceValue, i.Suit);
                tableCards.Add(card7);
                await Task.WhenAll(tableCard7.TranslateTo(2.2 * tableCard7.Width, -0.525 * tableCard7.Height, 500));
            }
            else if (!tableImgs.Contains(tableCard8))
            {
                tableCard8 = j;
                j.Clicked += OnTableImg8Clicked;
                j.Opacity = 1;
                tableImgs.Add(tableCard8);
                card8 = new Card(i.FaceValue, i.Suit);
                tableCards.Add(card8);
                await Task.WhenAll(tableCard8.TranslateTo(-2.2 * tableCard8.Width, 0.525 * tableCard8.Height, 500));
            }
            else if (!tableImgs.Contains(tableCard2))
            {
                tableCard2 = j;
                j.Clicked += OnTableImg2Clicked;
                j.Opacity = 1;
                tableImgs.Add(tableCard2);
                card2 = i;
                tableCards.Add(card2);
                await Task.WhenAll(tableCard2.TranslateTo(1.1 * tableCard2.Width, -0.525 * tableCard2.Height, 500));
            }
            else if (!tableImgs.Contains(tableCard10))
            {
                tableCard10 = j;
                j.Clicked += OnTableImg10Clicked;
                j.Opacity = 1;
                tableImgs.Add(tableCard10);
                card10 = i;
                tableCards.Add(card10);
                await Task.WhenAll(tableCard10.TranslateTo(3.3 * tableCard10.Width, 0.525 * tableCard10.Height, 500));
            }
            else if (!tableImgs.Contains(tableCard9))
            {
                tableCard9 = j;
                j.Clicked += OnTableImg9Clicked;
                j.Opacity = 1;
                tableImgs.Add(tableCard9);
                card9 = i;
                tableCards.Add(card9);
                await Task.WhenAll(tableCard9.TranslateTo(-3.3 * tableCard9.Width, 0.525 * tableCard9.Height, 500));
            }
            else if (!tableImgs.Contains(tableCard3))
            {
                tableCard3 = j;
                j.Clicked += OnTableImg3Clicked;
                j.Opacity = 1;
                tableImgs.Add(tableCard3);
                card3 = i;
                tableCards.Add(card3);
                await Task.WhenAll(tableCard3.TranslateTo(-1.1 * tableCard3.Width, 0.525 * tableCard3.Height, 500));
            }
            else if (!tableImgs.Contains(tableCard4))
            {
                tableCard4 = j;
                j.Clicked += OnTableImg4Clicked;
                j.Opacity = 1;
                tableImgs.Add(tableCard4);
                card4 = i;
                tableCards.Add(card4);
                await Task.WhenAll(tableCard4.TranslateTo(1.1 * tableCard4.Width, 0.525 * tableCard4.Height, 500));
            }
            else if (!tableImgs.Contains(tableCard1))
            {
                tableCard1 = j;
                j.Clicked += OnTableImg1Clicked;
                j.Opacity = 1;
                tableImgs.Add(tableCard1);
                card1 = i;
                tableCards.Add(card1);
                await Task.WhenAll(tableCard1.TranslateTo(-1.1 * tableCard1.Width, -0.525 * tableCard1.Height, 500));
            }
        }
        private async Task EndRound()
        {
            StatsPage statsPage = new StatsPage();
            Rectangle rect1 = new Rectangle (0.50, 0.50, 0.42, 0.25);
            nextRoundButton = new Button
            {
                Padding = 4,
                Text = "PLAY AGAIN",
                TextColor = Color.White,
                FontSize = 40,
                BorderWidth = 4,
                WidthRequest = 300,
                BorderColor = Color.White,
                Background = playButton.Background,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            nextRoundButton.Clicked += NextRound_Clicked;
            await Navigation.PushModalAsync(statsPage);
            humScoreLabel.Text = StatsPage.humanTotalScore.ToString();
            aiScoreLabel.Text = StatsPage.aiTotalScore.ToString();
            AbsoluteLayout.SetLayoutBounds(nextRoundButton, rect1);
            AbsoluteLayout.SetLayoutFlags(nextRoundButton, AbsoluteLayoutFlags.All);
            foreach (ImageButton i in tableImgs)
            {
                game.Children.Remove(i);
            }
            tableImgs.Clear();
            game.Children.Add(nextRoundButton);
        }
        private ImageSource CardID(Card card)
        {
            switch (card.FaceValue)
            {
                #region Aces
                case Card.Rank.Ace:
                    {
                        switch (card.Suit)
                        {
                            case Card.SuitType.Clubs:
                                {
                                    return ImageSource.FromResource("Scopa2.images.AC.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Diamonds:
                                {
                                    return ImageSource.FromResource("Scopa2.images.AD.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Hearts:
                                {
                                    return ImageSource.FromResource("Scopa2.images.AH.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Spades:
                                {
                                    return ImageSource.FromResource("Scopa2.images.AS.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly); ;
                                }
                            default:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Yellow_back.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                        }
                    }
                #endregion

                #region Two
                case Card.Rank.Two:
                    {
                        switch (card.Suit)
                        {
                            case Card.SuitType.Clubs:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Clubs2.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Diamonds:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Diamonds2.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Hearts:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Hearts2.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Spades:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Spades2.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            default:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Yellow_back.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                        }
                    }
                #endregion

                #region Three
                case Card.Rank.Three:
                    {
                        switch (card.Suit)
                        {
                            case Card.SuitType.Clubs:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Clubs3.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Diamonds:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Diamonds3.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Hearts:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Hearts3.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Spades:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Spades3.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            default:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Yellow_back.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly); ;
                                }
                        }
                    }
                #endregion

                #region Four
                case Card.Rank.Four:
                    {
                        switch (card.Suit)
                        {
                            case Card.SuitType.Clubs:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Clubs4.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Diamonds:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Diamonds4.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Hearts:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Hearts4.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Spades:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Spades4.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            default:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Yellow_back.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                        }
                    }
                #endregion

                #region Five
                case Card.Rank.Five:
                    {
                        switch (card.Suit)
                        {
                            case Card.SuitType.Clubs:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Clubs5.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Diamonds:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Diamonds5.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Hearts:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Hearts5.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Spades:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Spades5.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            default:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Yellow_back.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                        }
                    }
                #endregion

                #region Six
                case Card.Rank.Six:
                    {
                        switch (card.Suit)
                        {
                            case Card.SuitType.Clubs:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Clubs6.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Diamonds:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Diamonds6.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Hearts:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Hearts6.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Spades:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Spades6.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            default:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Yellow_back.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                        }
                    }
                #endregion

                #region Seven
                case Card.Rank.Seven:
                    {
                        switch (card.Suit)
                        {
                            case Card.SuitType.Clubs:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Clubs7.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Diamonds:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Diamonds7.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Hearts:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Hearts7.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Spades:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Spades7.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            default:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Yellow_back.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                        }
                    }

                #endregion

                #region Eight
                case Card.Rank.Eight:
                    {
                        switch (card.Suit)
                        {
                            case Card.SuitType.Clubs:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Clubs8.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Diamonds:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Diamonds8.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Hearts:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Hearts8.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Spades:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Spades8.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            default:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Yellow_back.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                        }
                    }

                #endregion

                #region Nine
                case Card.Rank.Nine:
                    {
                        switch (card.Suit)
                        {
                            case Card.SuitType.Clubs:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Clubs9.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Diamonds:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Diamonds9.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Hearts:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Hearts9.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Spades:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Spades9.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            default:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Yellow_back",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                        }
                    }
                #endregion

                #region Ten
                case Card.Rank.Ten:
                    {
                        switch (card.Suit)
                        {
                            case Card.SuitType.Clubs:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Clubs10.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Diamonds:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Diamonds10.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Hearts:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Hearts10.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            case Card.SuitType.Spades:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Spades10.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                            default:
                                {
                                    return ImageSource.FromResource("Scopa2.images.Yellow_back.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                                }
                        }
                    }
                #endregion
                default:
                    {
                        return ImageSource.FromResource("Scopa2.images.Yellow_back.jpg",
                    typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                    }
            }
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}