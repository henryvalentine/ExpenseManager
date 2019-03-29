using System;
using System.Globalization;

namespace xPlug.BusinessObject.CustomizedASPBusinessObject.PCVHelper
{
    public class PcvHelper
    {

        public string PcvGenerator(int pcvId, int zerosPrefix)
        {
            try
            {
                var pcvIdLenght = pcvId.ToString(CultureInfo.InvariantCulture).Length;

                if (pcvIdLenght < zerosPrefix)
                {
                    var zeroLenght = "";

                    for (int i = pcvIdLenght; i < zerosPrefix; i++)
                    {
                        zeroLenght += "0";
                    }

                    return zeroLenght + pcvId;
                }

                if (pcvIdLenght == zerosPrefix || pcvIdLenght > zerosPrefix)
                {
                    return pcvId.ToString(CultureInfo.InvariantCulture);

                }

                return pcvId.ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return pcvId.ToString(CultureInfo.InvariantCulture);
            }
        }

    }

}
