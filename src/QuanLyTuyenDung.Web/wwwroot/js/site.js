// Hỏi xác nhận trước khi gửi các form có thuộc tính data-xac-nhan (thường là nút Xóa)
document.addEventListener('submit', function (e) {
    const form = e.target;
    const thongDiep = form.getAttribute('data-xac-nhan');
    if (thongDiep && !confirm(thongDiep)) {
        e.preventDefault();
    }
});

// Tự ẩn thông báo thành công sau 4 giây
setTimeout(function () {
    document.querySelectorAll('.alert-success').forEach(function (el) {
        bootstrap.Alert.getOrCreateInstance(el).close();
    });
}, 4000);
