namespace Lexer
{
   public static class Lexer {
      public static IEnumerable<(Tokens, object? value)> ParseTokens(string input){
         var position = 0;
         while (position < input.Length){
            var c = input[position];

            if(char.IsWhiteSpace(c)){
               position ++;
               continue;
            }
            if(char.IsLetter(c)){
               var start = position;
               while(position < input.Length && (char.IsLetterOrDigit(input[position]))){
                  position ++;
               };
               var value = input.Substring(start, position - start);
               var token = value switch{
                  "let" => Tokens.Let,
                  "fn" => Tokens.Function,
                  _ => Tokens.Identifier
               };
               yield return (token, value);
               continue;
            }
            if(char.IsDigit(c)){
               var start = position;

               while (position < input.Length && char.IsDigit(input[position])){
                  position ++;
               }
               var value = input.Substring(start, position - start);
               yield return (Tokens.Integer, int.Parse(value));
            }

            if(c == '\"'){
               var start = position + 1;
               position ++;

               while (position < input.Length && input[position] != '\"'){
                  position ++;
               }

               if(position >= input.Length){
                  throw new Exception("unterminated string");
               }

               var value = input.Substring(start, position - start);
               position ++;

               yield return (Tokens.String, value);
               position ++;
               continue;
            }
            
            yield return c switch{
               '=' => (Tokens.Equals, '='),
               '+' => (Tokens.Plus, '+'),
               ',' => (Tokens.Comma, ','),
               ';' => (Tokens.Semicolon, ';'),
               '(' => (Tokens.LParen, '('),
               ')' => (Tokens.RParen, ')'),
               '{' => (Tokens.LSquirly, '{'),
               '}' => (Tokens.RSQuirly, '}'),
               _ => (Tokens.Illegal, c)
            };
            position ++;
         }

         yield return (Tokens.Eof, null);
      }
   }
}
