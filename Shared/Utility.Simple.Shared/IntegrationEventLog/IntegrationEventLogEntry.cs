using System;
using Newtonsoft.Json;
#if !(NET20 || NET30 || NET10 || NET11)
using System.Linq;
#endif
using Utility.EventBus.Events;

namespace Utility.IntegrationEventLog
{
    /// <summary>
    /// 集成 事件 日志 实体
    /// </summary>
    public class IntegrationEventLogEntry
    {
        private IntegrationEventLogEntry() { }
        /// <summary>
        /// 集成 事件 日志 实体
        /// </summary>
        /// <param name="event">集成 事件 </param>
        public IntegrationEventLogEntry(IntegrationEvent @event)
        {
            EventId = @event.Id;            
            CreationTime = @event.CreationDate;
            EventTypeName = @event.GetType().FullName;
            Content = JsonConvert.SerializeObject(@event);
            State = EventStateEnum.NotPublished;
            TimesSent = 0;
        }
        /// <summary>
        /// 事件 id
        /// </summary>
        public virtual string EventId { get; private set; }
        /// <summary>
        /// 事件 类型名称 反射 用的
        /// </summary>
        public virtual string EventTypeName { get; private set; }

        /// <summary>
        /// / 事件 类型 简单 名称
        /// </summary>
#if (NET20 || NET30 || NET10 || NET11)
        public virtual string EventTypeShortName
        {
            get
            {
                var strs = EventTypeName.Split('.');
                if (strs != null && strs.Length > 0)
                {
                    return strs[strs.Length - 1];
                }
                return null;
            }
        }

#else
        public virtual string EventTypeShortName => EventTypeName.Split('.')?.Last();
#endif
        /// <summary>
        /// 集成 事件
        /// </summary>
        public virtual IntegrationEvent IntegrationEvent { get; private set; }
        /// <summary>
        /// 事件 状态
        /// </summary>
        public virtual EventStateEnum State { get; set; }
        /// <summary>
        /// 事件 触发 次数
        /// </summary>
        public virtual int TimesSent { get; set; }
        /// <summary>
        /// 事件 创建 事件
        /// </summary>
        public virtual DateTime CreationTime { get; private set; }
        /// <summary>
        ///集成 事件  内容 即 sjon
        /// </summary>
        public virtual string Content { get; private set; }

        /// <summary>
        /// 更新 集成  事件  
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual IntegrationEventLogEntry DeserializeJsonContent(Type type)
        {
            IntegrationEvent = JsonConvert.DeserializeObject(Content, type) as IntegrationEvent;
            return this;
        }
    }
}
