namespace Utility.DAL
{
    /// <summary>
    /// 数据访问层 管理接口
    /// </summary>
    public interface IDALManager
    {
    }

    /// <summary>
    /// 数据访问层 管理接口
    /// </summary>
    public interface IManagerManager : IDALManager
    {
    }

    /// <summary>
    /// 数据访问层 管理接口
    /// </summary>
    public abstract class DALManager:IDALManager
    {
    }

    /// <summary>
    /// 数据访问层 管理接口
    /// </summary>
    public abstract class ManagerManager : DALManager, IManagerManager
    {
    }
}
