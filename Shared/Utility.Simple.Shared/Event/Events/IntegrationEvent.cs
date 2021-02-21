using System;
using Newtonsoft.Json;

namespace Utility.EventBus.Events
{
    /// <summary>
    /// 集成 事件 
    /// </summary>
    public class IntegrationEvent
    {
        /// <summary>
        /// 集成 事件 
        /// </summary>
        public IntegrationEvent()
        {
            Id = Guid.NewGuid().ToString();
            CreationDate = DateTime.UtcNow;
        }

        /// <summary>
        /// 集成 事件 
        /// </summary>
        [JsonConstructor]
        public IntegrationEvent(string id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }

        /// <summary>
        /// id
        /// </summary>
        [JsonProperty]
        public string Id { get; private set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonProperty]
        public DateTime CreationDate { get; private set; }
    }
}
