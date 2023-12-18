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

		public async Task<Character> GetCharacter(string id)
		{
			return await _characterRepository.GetCharacter(id);
		}

		public async Task<IEnumerable<Character>> GetCharacters(string charId)
		{
			return await _characterRepository.GetCharacters(charId);
		}
	}
}
