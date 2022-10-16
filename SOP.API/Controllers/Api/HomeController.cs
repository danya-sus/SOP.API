using Microsoft.AspNetCore.Mvc;

namespace SOP.API.Controllers.Api
{
    [Route("api")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var result = new
            {
                message = "Welcome to the SOP API!",
                _links = new
                {
                    vehicles = new
                    {
                        href = "/api/vehicles"
                    }
                }
            };

            return new JsonResult(result);
        }
    }
}

/*
AA07AMM,nissan-note,Turquoise,2007
AAC792H,hyundai-i10,Silver,1975
AAY452D,volkswagen-up,Purple,1979
AB01MFL,peugeot-bipper,Turquoise,2001
AB14GMD,jaguar-xf-series,Orange,2014
AB15UVT,vauxhall-viva,Grey,2015
AB19NWG,kia-sportage,Gold,2019
ABV496J,volvo-v40,Gold,1973
ABW940K,land-rover-range-rover,Red,1972
ABX857D,citroen-c4,Gold,1979
ABY761G,abarth-595,Gold,1976
AC03BXC,honda-jazz,Silver,2003
AC04VYH,mini-one,Black,2004
AC08RRN,peugeot-2008,Grey,2008
AC15VJX,vauxhall-astra,Blue,2015
ACD586D,peugeot-107,Black,1979
ACF678C,vauxhall-crossland,Green,1980
ACH210B,bmw-5-series,Grey,1981
ACK119D,ford-fiesta,Yellow,1979
ACR989D,ford-fiesta,Silver,1979
AD03RAM,vauxhall-corsa,Gold,2003
AD04RLG,volkswagen-tiguan,Gold,2004
AD05GDL,ford-fiesta,Red,2005
AD07VYV,kia-ceed,Purple,2007
AD09KHC,land-rover-discovery,Yellow,2009
AD16LVH,ford-fiesta,Purple,2016
ADN691F,volkswagen-golf,White,1977
ADV666G,land-rover-range-rover,Green,1976
ADY998J,suzuki-vitara,Grey,1973
AE03CHR,hyundai-i10,Yellow,2003
AE03MUU,ford-fiesta,Yellow,2003
AE09JVF,peugeot-206,Purple,2009
AE10BUA,ford-focus,Yellow,2010
AE11BUC,bmw-3-series,Red,2011
AEW457H,renault-kadjar,Turquoise,1975
AF02NCK,seat-leon,Yellow,2002
AF04DFE,audi-a1,Yellow,2004
AF06JXP,vauxhall-zafira,Black,2006
AF07YEG,kia-picanto,Purple,2007
AFA959K,ford-mondeo,Orange,1972
AFH963K,volkswagen-golf,Grey,1972
 */