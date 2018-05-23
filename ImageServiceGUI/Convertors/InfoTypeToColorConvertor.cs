using ImageService.Logging.Modal;
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
