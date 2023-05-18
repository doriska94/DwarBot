using Dwar.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Dwar.Http
{
    public class HpRepository : IHpRepository
    {
        private HttpRequest _httpRequest;
        private IDomain _domain;
        private const string UserHp = "/user.php?mode=personage&submode=backpack";
        public HpRepository(HttpRequest httpRequest, IDomain domain)
        {
            _httpRequest = httpRequest;
            _domain = domain;
        }

        public Hp Get()
        {
            var getUri = new Uri(_domain.GetBaseUri(), UserHp);
            var result = _httpRequest.GetRequest(getUri.AbsoluteUri);

            return GetHp(result);
        }

        public Hp GetHp(string response)
        {
            try
            {
                int indexOfActualHp = response.IndexOf("hp=");
                response = response.Remove(0, indexOfActualHp + 3);

                int hpLenght = response.IndexOf("&");
                var hp = response.Substring(0, hpLenght);

                int indexOfMaxHp = response.IndexOf("hpMax=");
                response = response.Remove(0, indexOfMaxHp + 6);
                int hpMaxLenght = response.IndexOf("&");
                var maxHp = response.Substring(0, hpMaxLenght);

                return new Hp(Convert.ToInt32(hp), Convert.ToInt32(maxHp));
            }
            catch { return new Hp (-1, -1); }
        }
    }
}
