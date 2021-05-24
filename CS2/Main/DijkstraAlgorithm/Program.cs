using System;
using System.Collections.Generic;
using System.Linq;
using Graphs;
using NUnit.Framework;

namespace HelloDijkstraAlgorithm
{
	public class Program
	{
		public static List<Node> DijkstraAlgorithm(
			Graph graph,
			Dictionary<Edge, double> weights,
			Node start,
			Node end,
			IPriorityQueue<Node> queue
			)
		{
			var track = new Dictionary<Node, Node>();
			track[start] = null;
			queue.Add(start, 0);

			while (true)
			{
				var toOpenPair = queue.ExtractMinKey();
				if (toOpenPair == null) return null;
				
				var toOpen = toOpenPair.Item1;
				var price = toOpenPair.Item2;
				
				if (toOpen == end) break;

				foreach (var e in toOpen.IncidentEdges.Where(z => z.From == toOpen))
				{
					var currentPrice = price + weights[e];
					var nextNode = e.OtherNode(toOpen);
					if (queue.UpdateOrAdd(nextNode, currentPrice))
						track[nextNode] = toOpen;
				}
			}

			var result = new List<Node>();
			while (end != null)
			{
				result.Add(end);
				end = track[end];
			}
			result.Reverse();
			return result;
		}

		public static void Main()
		{
			var graph = new Graph(4);
			var weights = new Dictionary<Edge, double>();
			weights[graph.Connect(0, 1)] = 1;
			weights[graph.Connect(0, 2)] = 2;
			weights[graph.Connect(0, 3)] = 6;
			weights[graph.Connect(1, 3)] = 4;
			weights[graph.Connect(2, 3)] = 2;
		
			var path = DijkstraAlgorithm(
				graph,
				weights,
				graph[0],
				graph[3],
				new DictionaryPriorityQueue<Node>()
				);
			//foreach (var e in path)
			//	Console.WriteLine(e.NodeNumber);
			CollectionAssert.AreEqual(new[] { 0, 2, 4 }, path.Select(z=>z.NodeNumber).ToArray());
		}
	}
}
