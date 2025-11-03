# Hướng dẫn cải thiện giao diện

## Các bước đã thực hiện:

### 1. ✅ Tạo ThemeHelper class
- File: `WindowsFormsApp1/UI/ThemeHelper.cs`
- Quản lý màu sắc, fonts, và style chung cho toàn bộ ứng dụng
- Các màu chính:
  - PrimaryBlue: #2196F3
  - HeaderBlue: #1976D2
  - SidebarBackground: #F0F2F5
  - BackgroundLight: #F5F5F5

### 2. ✅ Cải thiện LoginForm
- Apply theme với màu sắc và fonts hiện đại
- Style cho buttons, textboxes, labels

### 3. 🔄 Đang thực hiện: Cải thiện StudentMainForm và InstructorMainForm
- Thêm Sidebar navigation (bên trái)
- Thêm Header bar (màu xanh đậm ở trên)
- Customize DataGridView với header màu xanh
- Style các buttons với rounded corners
- Layout hiện đại hơn

## Cấu trúc layout mới:

```
┌─────────────────────────────────────────┐
│  HEADER BAR (màu xanh đậm)              │
├──────────┬──────────────────────────────┤
│          │                              │
│ SIDEBAR  │   MAIN CONTENT AREA         │
│ (xám nhạt)│   (trắng)                   │
│          │                              │
│ - Menu   │   - Tables                   │
│ - Info   │   - Buttons                  │
│ - Logout │   - Forms                    │
│          │                              │
└──────────┴──────────────────────────────┘
```

## Tiếp theo:

1. Hoàn thiện StudentMainForm với sidebar và header
2. Áp dụng tương tự cho InstructorMainForm
3. Customize DataGridView
4. Cải thiện các form con (StudentsListForm, etc.)


