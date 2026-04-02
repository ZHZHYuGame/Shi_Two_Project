using System;
using System.Threading;

/// <summary>
/// 雪花算法 ID 生成器（64位）
/// 结构：1位符号位 + 41位时间戳 + 10位机器ID + 12位序列号
/// </summary>
public class SnowflakeIdGenerator
{
    // 起始时间戳（2020-01-01 00:00:00），可自定义
    private const long Twepoch = 1577836800000L;

    // 机器ID所占位数
    private const int MachineIdBits = 10;
    // 序列号所占位数
    private const int SequenceBits = 12;

    // 最大机器ID（2^10 -1 = 1023）
    private const long MaxMachineId = (1L << MachineIdBits) - 1;
    // 最大序列号（2^12 -1 = 4095）
    private const long MaxSequence = (1L << SequenceBits) - 1;

    // 左移位数（用于位运算）
    private const int MachineIdShift = SequenceBits;
    private const int TimestampShift = SequenceBits + MachineIdBits;

    private readonly long _machineId;
    private long _lastTimestamp = -1L;
    private long _sequence = 0L;

    private readonly object _lock = new object();

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="machineId">机器ID（0 ~ 1023）</param>
    public SnowflakeIdGenerator(long machineId)
    {
        if (machineId < 0 || machineId > MaxMachineId)
        {
            throw new ArgumentException($"MachineId must be between 0 and {MaxMachineId}");
        }
        _machineId = machineId;
    }

    /// <summary>
    /// 生成下一个唯一ID
    /// </summary>
    public long NextId()
    {
        lock (_lock)
        {
            var timestamp = GetCurrentTimestamp();

            // 时钟回拨处理
            if (timestamp < _lastTimestamp)
            {
                // 如果回拨较小，等待直到追上；如果回拨较大，可抛出异常或重启服务
                var offset = _lastTimestamp - timestamp;
                if (offset <= 5)
                {
                    // 等待一小段时间（最多5ms）让时钟追上
                    Thread.Sleep((int)(offset * 2));
                    timestamp = GetCurrentTimestamp();
                    if (timestamp < _lastTimestamp)
                    {
                        throw new Exception("Clock moved backwards. Refusing to generate id.");
                    }
                }
                else
                {
                    throw new Exception($"Clock moved backwards too much. Refusing to generate id. offset={offset}");
                }
            }

            // 同一毫秒内，序列号自增
            if (timestamp == _lastTimestamp)
            {
                _sequence = (_sequence + 1) & MaxSequence;
                if (_sequence == 0)
                {
                    // 序列号用尽，等待下一毫秒
                    timestamp = WaitNextMillisecond(_lastTimestamp);
                }
            }
            else
            {
                _sequence = 0L;
            }

            _lastTimestamp = timestamp;

            // 组合ID：时间戳部分 + 机器ID部分 + 序列号部分
            var id = ((timestamp - Twepoch) << TimestampShift)
                     | (_machineId << MachineIdShift)
                     | _sequence;
            return id;
        }
    }

    /// <summary>
    /// 等待下一毫秒
    /// </summary>
    private long WaitNextMillisecond(long lastTimestamp)
    {
        var timestamp = GetCurrentTimestamp();
        while (timestamp <= lastTimestamp)
        {
            timestamp = GetCurrentTimestamp();
        }
        return timestamp;
    }

    /// <summary>
    /// 获取当前毫秒时间戳
    /// </summary>
    private long GetCurrentTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}