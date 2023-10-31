using DataAccessMD;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
	public class CharacterBL
	{
		private readonly CharacterMD _characterMD;
		public CharacterBL(CharacterMD characterMD) 
		{
			_characterMD = characterMD;
		}

		public async Task<Character> StoreCharacter(Character character)
		{
			return await _characterMD.Create(character);
		}
	}
}
