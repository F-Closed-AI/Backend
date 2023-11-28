using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Application.Models;
using WebApi.Application.Repositories;

namespace WebApi.Application.Services
{
	public class CharacterService
	{
		private readonly CharacterRepository _characterRepository;
		public CharacterService(CharacterRepository characterRepository)
		{
			_characterRepository = characterRepository;
		}

		public async Task<Character> StoreCharacter(Character character)
		{
			return await _characterRepository.StoreCharacter(character);
		}

		public async Task<List<Character>> GetCharacter(string charId)
		{
			return await _characterRepository.GetCharacter(charId);
		}
	}
}
