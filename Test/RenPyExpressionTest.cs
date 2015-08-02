#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using DPek.Raconteur.Util.Expressions;

namespace DPek.Raconteur.Test
{
	[InitializeOnLoad]
	public class RenPyExpressionTest : EditorWindow
	{
		public static bool NoOpInt()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("1234");
			Value val = expression.Evaluate(state);

			return (int?)val.GetRawValue(state) == 1234;
		}

		// TODO: How does Ren'Py deal with doubles?
		public static bool NoOpFloat()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("1234.56");
			Value val = expression.Evaluate(state);

			return (float?)val.GetRawValue(state) == 1234.56f;
		}

		public static bool NoOpVariable()
		{
			var state = new TestStoryState();
			state.SetVariable("variable", "test");

			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("variable");
			Value val = expression.Evaluate(state);

			return (string)val.GetRawValue(state) == "test";
		}

		public static bool EqualSuccess()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("1234 == 1234");
			Value val = expression.Evaluate(state);

			return true == (bool)val.GetRawValue(state);
		}

		public static bool EqualFail()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("1234 == 4343");
			Value val = expression.Evaluate(state);

			return false == (bool)val.GetRawValue(state);
		}

		public static bool NotEqualSuccess()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("1234 != 4343");
			Value val = expression.Evaluate(state);

			return true == (bool)val.GetRawValue(state);
		}

		public static bool NotEqualFail()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("1234 != 1234");
			Value val = expression.Evaluate(state);

			return false == (bool)val.GetRawValue(state);
		}

		public static bool LessThanSuccess()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("1234 < 12345");
			Value val = expression.Evaluate(state);

			return true == (bool)val.GetRawValue(state);
		}

		public static bool LessThanFail()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("1234 < 1234");
			Value val = expression.Evaluate(state);

			return false == (bool)val.GetRawValue(state);
		}

		public static bool LessThanFail2()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("1234 < 123");
			Value val = expression.Evaluate(state);

			return false == (bool)val.GetRawValue(state);
		}

		public static bool LessThanEqualSuccess()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("1234 <= 12345");
			Value val = expression.Evaluate(state);

			return true == (bool)val.GetRawValue(state);
		}

		public static bool LessThanEqualSuccess2()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("1234 <= 1234");
			Value val = expression.Evaluate(state);

			return true == (bool)val.GetRawValue(state);
		}

		public static bool LessThanEqualFail()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("1234 <= 123");
			Value val = expression.Evaluate(state);

			return false == (bool)val.GetRawValue(state);
		}

		public static bool GreaterThanSuccess()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("12345 > 1234");
			Value val = expression.Evaluate(state);

			return true == (bool)val.GetRawValue(state);
		}

		public static bool GreaterThanFail()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("1234 > 1234");
			Value val = expression.Evaluate(state);

			return false == (bool)val.GetRawValue(state);
		}

		public static bool GreaterThanFail2()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("123 > 1234");
			Value val = expression.Evaluate(state);

			return false == (bool)val.GetRawValue(state);
		}

		public static bool GreaterThanEqualSuccess()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("12345 >= 1234");
			Value val = expression.Evaluate(state);

			return true == (bool)val.GetRawValue(state);
		}

		public static bool GreaterThanEqualSuccess2()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("1234 >= 1234");
			Value val = expression.Evaluate(state);

			return true == (bool)val.GetRawValue(state);
		}

		public static bool GreaterThanEqualFail()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("123 >= 1234");
			Value val = expression.Evaluate(state);

			return false == (bool)val.GetRawValue(state);
		}

		public static bool Addition()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("1 + 5");
			Value val = expression.Evaluate(state);

			return 6 == (int)val.GetRawValue(state);
		}

		public static bool AdditionNegatives()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("-1 + 5");
			Value val = expression.Evaluate(state);

			return 4 == (int)val.GetRawValue(state);
		}

		public static bool Subtraction()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("1 - 5");
			Value val = expression.Evaluate(state);

			return -4 == (int)val.GetRawValue(state);
		}

		public static bool SubtractionNegatives()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("-1 - 5");
			Value val = expression.Evaluate(state);

			return -6 == (int)val.GetRawValue(state);
		}

		public static bool Multiplication()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("2 * 5");
			Value val = expression.Evaluate(state);

			return 10 == (int)val.GetRawValue(state);
		}

		public static bool MultiplicationNegatives()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("-2 * 5");
			Value val = expression.Evaluate(state);

			return -10 == (int)val.GetRawValue(state);
		}

		public static bool Division()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("10 / 5");
			Value val = expression.Evaluate(state);

			return 2 == (int)val.GetRawValue(state);
		}

		public static bool DivisionNegatives()
		{
			var state = new TestStoryState();
			var parser = ExpressionParserFactory.GetRenPyParser();

			var expression = parser.ParseExpression("10 / -5");
			Value val = expression.Evaluate(state);

			return -2 == (int)val.GetRawValue(state);
		}
	}
}
#endif
