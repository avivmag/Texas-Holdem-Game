using System;
using System.Collections.Generic;

namespace BL.Game
{
	public class Deck
	{
		private List<Card> cards;
		private Random rand;

		public Deck()
		{
			rand = new Random();
			Shuffle();
		}
		public Card Top()
		{
			Card ans = cards[0];
			cards.RemoveAt(0);
			return ans;
		}
		public void Shuffle()
		{
			List<Card> temp = new List<Card>();
			for (int i = Card.LOWEST_CARD; i <= Card.HIGHEST_CARD; i++)
			{
				temp.Add(new Card(Card.cardType.club, i));
				temp.Add(new Card(Card.cardType.diamond, i));
				temp.Add(new Card(Card.cardType.heart, i));
				temp.Add(new Card(Card.cardType.spade, i));
			}
			cards = new List<Card>();
			int size = Card.NUMBER_OF_CARDS;

			while (size != 0)
			{
				int index = rand.Next(0, size - 1);
				cards.Add(temp[index]);
				temp.RemoveAt(index);
				size--;
			}
		}
	}
}
