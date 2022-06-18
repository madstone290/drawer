using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models.Authentication
{
    public class RefreshToken 
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; init; } = default!;

        /// <summary>
        /// 사용자Id
        /// </summary>
        public string UserId { get; init; } = default!;

        /// <summary>
        /// 토큰 값
        /// </summary>
        public string Token { get; init; } = default!;
        
        /// <summary>
        /// UTC 만료시간
        /// </summary>
        public DateTime UtcExpires { get; init; }

        /// <summary>
        /// 로컬 만료시간
        /// </summary>
        public DateTime LocalExpires => UtcExpires.ToLocalTime();

        /// <summary>
        /// 만료 여부
        /// </summary>
        public bool IsExpired => DateTime.UtcNow >= UtcExpires;

        /// <summary>
        /// 생성시간
        /// </summary>
        public DateTime Created { get; init; }

        /// <summary>
        /// 취소시간
        /// </summary>
        public DateTime? Revoked { get; private set; }

        /// <summary>
        /// 활성화 여부
        /// </summary>
        public bool IsActive => Revoked == null && !IsExpired;

        private RefreshToken() { }
        public RefreshToken(string userId, string token, TimeSpan lifetime)
        {
            Id = Guid.NewGuid().ToString();
            UserId = userId;
            Token = token;
            Created = DateTime.UtcNow;
            UtcExpires = Created.Add(lifetime);
        }

        /// <summary>
        /// 토큰을 무효화한다.
        /// </summary>
        public void Revoke()
        {
            Revoked = DateTime.UtcNow;
        }
    }
}
