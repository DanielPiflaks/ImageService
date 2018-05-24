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
    class InfoTypeToColorConvertor : IValueConverter
    {
        /// <summary>
        /// Convert message from enum type to color.
        /// </summary>
        /// <param name="value"> received value.</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns> fitting color for xaml.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((MessageTypeEnum)value)
            {
                case MessageTypeEnum.FAIL:
                    return "Red";
                case MessageTypeEnum.INFO:
                    return "Green";
                case MessageTypeEnum.WARNING:
                    return "Yellow";
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
