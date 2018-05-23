using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ImageServiceGUI.Convertors
{
    class ConnectedConvertor : IValueConverter
    {
        /// <summary>
        /// Convert enum value to color.
        /// </summary>
        /// <param name="value"> received value</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((IsConnectedEnum)value)
            {
                // If connection to server succesed.
                case IsConnectedEnum.Connected:
                    return "Transparent";
                    // If connection to server failed.
                case IsConnectedEnum.NotConnected:
                    return "Gray";
                default:
                    return "Transparent";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
