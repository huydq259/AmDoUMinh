# 📖 Tài Liệu Dự Án Unity - AmDoUMinh

## 📋 Mục Lục
1. [Tổng Quan](#tổng-quan)
2. [Player Scripts](#player-scripts)
3. [Enemy Scripts](#enemy-scripts)
4. [Game Management](#game-management)
5. [Audio System](#audio-system)
6. [Utility Scripts](#utility-scripts)
7. [Trap & Environment](#trap--environment)

---

## 🎮 Tổng Quan

Đây là một dự án game 2D platformer được phát triển bằng Unity. Game bao gồm các tính năng:
- Nhân vật người chơi có thể di chuyển, nhảy, bắn tên, và dash
- Nhiều loại kẻ địch với AI khác nhau
- Hệ thống bẫy và chướng ngại vật
- Hệ thống âm thanh và hiệu ứng camera

---

## 🎯 Player Scripts

### 1. Player.cs
**Mô tả:** Script chính điều khiển nhân vật người chơi.

| Hàm | Chức Năng |
|-----|-----------|
| `Start()` | Khởi tạo các biến, thiết lập instance singleton, reset trạng thái ban đầu |
| `Update()` | Xử lý input người chơi, kiểm tra va chạm mặt đất, gọi các hàm di chuyển và animation |
| `FixedUpdate()` | Cập nhật velocity của Rigidbody2D theo input di chuyển ngang |
| `FireArrow()` | Tạo và bắn mũi tên từ vị trí spawn với tốc độ đã định |
| `PlayRunAnimation()` | Điều khiển animation chạy dựa trên giá trị di chuyển |
| `Flip()` | Xoay nhân vật theo hướng di chuyển (trái/phải) |
| `Jump()` | Xử lý logic nhảy với hệ thống double jump |
| `TakeDamage(int damageAmount)` | Nhận sát thương, phát animation bị thương, rung camera |
| `OnCollisionEnter2D(Collision2D)` | Xử lý va chạm với mặt đất, reset số lượt nhảy |
| `OnTriggerEnter2D(Collider2D)` | Xử lý trigger với: DeadLine, Trap, Chest, Heart, Arrow, Diamond |
| `OnDrawGizmosSelected()` | Vẽ gizmo hiển thị vùng kiểm tra mặt đất trong Editor |
| `Die()` | Xử lý chết: phát âm thanh, tạo hiệu ứng nổ, hiện UI Game Over |

**Biến quan trọng:**
- `maxHealth`: Máu tối đa
- `jumpHeight`: Độ cao nhảy
- `moveSpeed`: Tốc độ di chuyển
- `totallJumps`: Tổng số lượt nhảy (double jump)

---

### 2. PlayerDash.cs
**Mô tả:** Xử lý kỹ năng dash (lướt nhanh) của người chơi.

| Hàm | Chức Năng |
|-----|-----------|
| `Start()` | Lấy reference đến Rigidbody2D, Animator, Player; lưu gravity gốc |
| `Update()` | Kiểm tra hướng di chuyển, xử lý input dash (chuột phải/Backspace) |
| `Dash()` | Coroutine thực hiện dash: tắt gravity, đẩy nhân vật theo hướng, khôi phục sau duration |
| `OnCollisionEnter2D(Collision2D)` | Tắt animation dash khi chạm đất |

**Biến quan trọng:**
- `dashForce`: Lực đẩy khi dash
- `dashDuration`: Thời gian dash kéo dài

---

## 👾 Enemy Scripts

### 3. Enemy.cs
**Mô tả:** Kẻ địch cận chiến có AI tuần tra và đuổi theo người chơi.

| Hàm | Chức Năng |
|-----|-----------|
| `Start()` | Khởi tạo components, tìm reference đến Player |
| `Update()` | Logic AI: tuần tra khi không phát hiện player, đuổi theo và tấn công khi trong tầm |
| `Attack()` | Gây sát thương cho player nếu trong phạm vi tấn công |
| `TakeDamage(int damageAmount)` | Nhận sát thương, phát animation hurt, rung camera, hiện floating text |
| `OnDrawGizmosSelected()` | Vẽ các gizmo debug: tầm phát hiện, điểm kiểm tra đất, tầm tấn công |
| `OnTriggerEnter2D(Collider2D)` | Xử lý bị bắn trúng bởi mũi tên |
| `OnCollisionEnter2D(Collision2D)` | Quay đầu khi va chạm với vật thể có Rigidbody |
| `ShakeCamera()` | Rung camera với cường độ và thời gian xác định |
| `Die()` | Xử lý chết: log, animation, tắt physics, hủy object sau 5s |

**AI Logic:**
1. **Tuần tra:** Di chuyển qua lại, quay đầu khi hết đường hoặc gặp tường
2. **Phát hiện:** Sử dụng OverlapCircle để phát hiện player trong `attackRangeRadius`
3. **Đuổi theo:** Di chuyển về phía player khi trong tầm
4. **Tấn công:** Khi khoảng cách <= `retrieveDistance`

---

### 4. Enemy2.cs
**Mô tả:** Kẻ địch bắn tên từ xa (không di chuyển).

| Hàm | Chức Năng |
|-----|-----------|
| `Start()` | Khởi tạo components, tìm reference đến Player |
| `Update()` | Logic AI: xoay về hướng player, kích hoạt animation tấn công khi trong tầm |
| `FireArrow()` | Tạo và bắn mũi tên về phía player |
| `TakeDamage(int damageAmount)` | Nhận sát thương, phát animation hurt, rung camera |
| `OnDrawGizmosSelected()` | Vẽ gizmo tầm tấn công |
| `OnTriggerEnter2D(Collider2D)` | Xử lý bị bắn trúng bởi mũi tên |
| `ShakeCamera()` | Rung camera |
| `Die()` | Xử lý chết |

---

### 5. bo.cs
**Mô tả:** Kẻ địch tuần tra với AI đơn giản hơn.

| Hàm | Chức Năng |
|-----|-----------|
| `Update()` | Logic AI: phát hiện player, đuổi theo hoặc tuần tra, tấn công khi đủ gần |
| `OnDrawGizmosSelected()` | Vẽ gizmo raycast kiểm tra mặt đất và tầm tấn công |

**Đặc điểm:**
- Di chuyển liên tục theo hướng hiện tại
- Raycast xuống dưới để phát hiện mép platform
- Đuổi theo player khi trong `attackRange`
- Tấn công khi khoảng cách <= `retrieveDistance`

---

### 6. Arrow.cs
**Mô tả:** Script cho đạn mũi tên.

| Hàm | Chức Năng |
|-----|-----------|
| `Start()` | Tự hủy sau 5 giây |
| `OnTriggerEnter2D(Collider2D)` | Hủy mũi tên khi chạm Ground |

---

## 🎛️ Game Management

### 7. GameManager.cs
**Mô tả:** Quản lý trạng thái game và UI.

| Hàm | Chức Năng |
|-----|-----------|
| `Awake()` | Thiết lập singleton instance |
| `Start()` | Khởi tạo số key = 0, ẩn UI Game Over |
| `TriggerGameOverUI()` | Hiện UI Game Over với animation bounce, phát âm thanh |
| `TriggerVictoryUI()` | Hiện UI Victory với animation |

**Biến quan trọng:**
- `key`: Số lượng chìa khóa (collectible)
- `gameOverUIBG`: Reference đến UI Game Over
- `victoryUIBG`: Reference đến UI Victory

---

### 8. SceneManagement.cs
**Mô tả:** Quản lý việc chuyển scene/màn chơi.

| Hàm | Chức Năng |
|-----|-----------|
| `Awake()` | Thiết lập singleton |
| `PlayClickSound()` | Phát âm thanh click |
| `LoadLevel(string tenManChoi)` | Load scene theo tên, reset Time.timeScale |
| `SmartPlayButton()` | Nút Play thông minh: vào màn 1 từ Menu, hoặc retry màn hiện tại |
| `Retry()` | Load lại scene hiện tại |
| `Menu()` | Quay về scene Menu |
| `ExitGame()` | Thoát ứng dụng |
| `NextLevel()` | Chuyển sang màn tiếp theo theo thứ tự: man1 → Scene3.1 → SampleScene → Menu |

---

## 🔊 Audio System

### 9. AudioManager.cs
**Mô tả:** Quản lý toàn bộ âm thanh trong game.

| Hàm | Chức Năng |
|-----|-----------|
| `Awake()` | Thiết lập singleton, DontDestroyOnLoad để giữ qua các scene |
| `Start()` | Tạo AudioSource cho mỗi Sound trong mảng, phát nhạc nền "BGM" |
| `PlaySound(string name)` | Phát âm thanh theo tên |

**Cách sử dụng:**
```csharp
AudioManager.instance.PlaySound("TenAmThanh");
```

---

### 10. Sound.cs
**Mô tả:** Class serializable định nghĩa một âm thanh.

| Thuộc Tính | Mô Tả |
|------------|-------|
| `clip` | AudioClip chứa file âm thanh |
| `name` | Tên để gọi âm thanh |
| `volume` | Âm lượng (0-1) |
| `loop` | Có lặp lại không |
| `source` | AudioSource được tạo runtime |

---

### 11. TrapsSound.cs
**Mô tả:** Phát âm thanh bẫy khi player ở gần.

| Hàm | Chức Năng |
|-----|-----------|
| `PlayElectricTrapSound()` | Phát âm thanh điện giật nếu player trong phạm vi |
| `PlayExplosionSound()` | Phát âm thanh nổ nếu player trong phạm vi |
| `PlayHushSound()` | (Chưa implement) Phát âm thanh gió |
| `OnDrawGizmosSelected()` | Vẽ gizmo phạm vi âm thanh |

---

## 🛠️ Utility Scripts

### 12. CameraShake.cs
**Mô tả:** Hiệu ứng rung camera sử dụng Cinemachine.

| Hàm | Chức Năng |
|-----|-----------|
| `Start()` | Thiết lập singleton |
| `Update()` | Đếm ngược thời gian rung, tắt hiệu ứng khi hết |
| `Shake(float intensity, float duration)` | Kích hoạt rung camera với cường độ và thời gian |

**Cách sử dụng:**
```csharp
CameraShake.instance.Shake(2.5f, 0.15f);
```

---

### 13. FloatingText.cs
**Mô tả:** Hiển thị số damage ngẫu nhiên nổi lên.

| Hàm | Chức Năng |
|-----|-----------|
| `Start()` | Random số 1-100, hiển thị lên TextMesh, tự hủy sau 1.01s |

---

## 🏔️ Trap & Environment

### 14. Saw.cs (SawMover)
**Mô tả:** Điều khiển chuyển động của cưa (bẫy di động).

| Hàm | Chức Năng |
|-----|-----------|
| `Start()` | Bắt đầu di chuyển đến điểm đầu tiên |
| `MoveToNextPoint()` | Di chuyển đến điểm tiếp theo trong mảng points, lặp vòng, sử dụng LeanTween |

**Cách thiết lập:**
1. Tạo các Transform điểm đích
2. Gán vào mảng `points`
3. Điều chỉnh `moveTime` cho tốc độ di chuyển

---

### 15. OneWayPlatform.cs
**Mô tả:** Platform một chiều (có thể nhảy xuyên từ dưới lên).

| Hàm | Chức Năng |
|-----|-----------|
| `Start()` | Lấy BoxCollider2D, tắt collider, tìm Player |
| `Update()` | Bật/tắt collider dựa vào vị trí Y của player so với platform |

**Logic:**
- Nếu player ở **trên** platform (position.y > platform.y + offset): Bật collider
- Nếu player ở **dưới** platform: Tắt collider để có thể nhảy xuyên qua

---

## 📊 Sơ Đồ Quan Hệ

```
┌─────────────────────────────────────────────────────────────┐
│                        GameManager                          │
│  (Quản lý trạng thái game, UI Game Over, Victory)          │
└─────────────────────────────────────────────────────────────┘
                              │
        ┌─────────────────────┼─────────────────────┐
        ▼                     ▼                     ▼
┌───────────────┐    ┌───────────────┐    ┌───────────────┐
│    Player     │◄──►│AudioManager   │◄──►│  SceneManager │
│  + PlayerDash │    │   + Sound     │    │               │
└───────────────┘    └───────────────┘    └───────────────┘
        │
        │ Tương tác
        ▼
┌───────────────────────────────────────┐
│           Enemies                      │
│  ┌─────────┐ ┌─────────┐ ┌─────────┐  │
│  │ Enemy   │ │ Enemy2  │ │   bo    │  │
│  │(Cận c.) │ │(Xa)     │ │(Tuần t.)│  │
│  └─────────┘ └─────────┘ └─────────┘  │
└───────────────────────────────────────┘
        │
        │ Sử dụng
        ▼
┌───────────────────────────────────────┐
│         Utilities                      │
│  ┌────────────┐ ┌────────────────┐    │
│  │CameraShake │ │ FloatingText   │    │
│  └────────────┘ └────────────────┘    │
└───────────────────────────────────────┘
```

---

## 🏷️ Tags Được Sử Dụng

| Tag | Mô tả |
|-----|-------|
| `Player` | Nhân vật người chơi |
| `Ground` | Mặt đất/platform |
| `DeadLine` | Vùng chết (rơi xuống vực) |
| `Trap` | Bẫy gây sát thương |
| `Chest` | Rương kho báu (victory trigger) |
| `Heart` | Vật phẩm hồi máu |
| `Diamond` | Kim cương (collectible) |
| `Arrow` | Mũi tên của player |
| `Arrow_Enemy` | Mũi tên của enemy |

---

## 🎵 Danh Sách Âm Thanh

| Tên | Sử Dụng |
|-----|---------|
| `BGM` | Nhạc nền |
| `Click` | Âm thanh click button |
| `Dash` | Âm thanh dash và nhảy |
| `Collect` | Thu thập item |
| `Electric` | Bẫy điện |
| `Explosion` | Nổ |
| `Game Over` | Thua game |

---

## 📝 Ghi Chú Phát Triển

- **Singleton Pattern**: Được sử dụng ở `Player`, `GameManager`, `AudioManager`, `SceneManagement`, `CameraShake`
- **LeanTween**: Sử dụng cho animation UI và di chuyển Saw
- **Cinemachine**: Sử dụng cho hiệu ứng rung camera
- **Double Jump**: Player có thể nhảy 2 lần (có thể điều chỉnh `totallJumps`)

---

*Tài liệu được tạo tự động - Cập nhật ngày 13/01/2026*
