using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Clientes.Infrastructure.Logging
{
    public sealed class MaskingEnricher : ILogEventEnricher
    {
        private readonly string _mask = "*****";
        private readonly string[] _propertiresToMask = { "password", "token", "secret", "authorization" };

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            foreach (var property in logEvent.Properties.ToList())
            {
                LogEventPropertyValue logEventPropertyValue = MaskProperty(property);

                logEvent.AddOrUpdateProperty(
                        new LogEventProperty(
                            property.Key,
                            logEventPropertyValue!));
            }
        }

        private LogEventPropertyValue MaskProperty(KeyValuePair<string, LogEventPropertyValue> property)
        {
            switch (property.Value)
            {
                case ScalarValue:
                    {
                        if (_propertiresToMask.Contains(property.Key.ToLower()))
                            return new ScalarValue(_mask);
                        else
                            return property.Value;
                    }
                case DictionaryValue dictionaryValue:
                    {
                        var resultDictionary = new List<KeyValuePair<ScalarValue, LogEventPropertyValue>>();

                        foreach (var pair in dictionaryValue.Elements)
                        {
                            var pairResult = MaskProperty(new KeyValuePair<string, LogEventPropertyValue>(pair.Key.Value as string, pair.Value));

                            resultDictionary.Add(new KeyValuePair<ScalarValue, LogEventPropertyValue>(pair.Key, pairResult));
                        }

                        return new DictionaryValue(resultDictionary);
                    }
                case SequenceValue sequenceValue:
                    var resultElements = new List<LogEventPropertyValue>();

                    foreach (var element in sequenceValue.Elements)
                    {
                        var elementResult = MaskProperty(new KeyValuePair<string, LogEventPropertyValue>(property.Key, element));

                        resultElements.Add(elementResult!);
                    }

                    return new SequenceValue(resultElements);
                case StructureValue structureValue:
                    {
                        var propList = new List<LogEventProperty>();

                        foreach (var prop in structureValue.Properties)
                        {
                            var maskedValue = MaskProperty(new KeyValuePair<string, LogEventPropertyValue>(prop.Name, prop.Value));

                            propList.Add(new LogEventProperty(prop.Name, maskedValue!));
                        }

                        return new StructureValue(propList);
                    }
                default:
                    return property.Value;
            }
        }
    }
}
