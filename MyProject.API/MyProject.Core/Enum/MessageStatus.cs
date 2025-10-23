using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Core.Enum
{
    public enum MessageStatus
    {
        /// <summary>
        /// PENDING: Tin nhắn đã được tạo nhưng đang chờ để được gửi đi.
        /// Nó có thể đang đợi kết nối mạng hoặc đang trong hàng đợi ưu tiên thấp.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// SENDING: Tin nhắn đang trong quá trình được gửi đi từ thiết bị của bạn.
        /// </summary>
        Sending = 1,

        /// <summary>
        /// SENT: Tin nhắn đã được gửi thành công từ thiết bị của bạn và đã đến được máy chủ.
        /// (Tương đương với 1 dấu tick ✓ trong các ứng dụng chat).
        /// </summary>
        Sent = 2,

        /// <summary>
        /// DELIVERED: Tin nhắn đã được giao thành công đến thiết bị của người nhận.
        /// Người nhận chưa nhất thiết phải đọc nó.
        /// (Tương đương với 2 dấu tick ✓✓).
        /// </summary>
        Delivered = 3,

        /// <summary>
        /// READ: Người nhận đã mở và xem tin nhắn.
        /// (Tương đương với 2 dấu tick màu xanh ✓✓).
        /// </summary>
        Read = 4,

        /// <summary>
        /// FAILED: Việc gửi tin nhắn đã thất bại vì một lý do nào đó (mất mạng, lỗi máy chủ, v.v.).
        /// Cần có thêm thông tin để biết lý do thất bại cụ thể.
        /// </summary>
        Failed = 5

    }
}
