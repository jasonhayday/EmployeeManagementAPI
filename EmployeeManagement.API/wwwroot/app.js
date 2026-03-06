const API = "/api/employees";

let employees = [];
let filteredEmployees = [];
let currentPage = 1;
const perPage = 10;
let sortField = null;
let sortAsc = true;

function showLoading(show) {
    document.getElementById("loading").style.display = show ? "flex" : "none";
}

function toast(msg, type = "success") {
    const t = document.getElementById("toast");
    t.innerText = msg;
    t.style.background = type === "error" ? "#e74c3c" : "#111";
    t.style.display = "block";

    setTimeout(() => (t.style.display = "none"), 3000);
}

function formatRupiah(num) {
    return "Rp " + Number(num).toLocaleString("id-ID");
}

async function loadEmployees() {
    showLoading(true);

    const res = await fetch(API);
    employees = await res.json();

    filteredEmployees = [...employees];
    currentPage = 1;
    showLoading(false);
    renderTable();
}

function renderTable() {
    let data = [...filteredEmployees];

    if (sortField) {
        data.sort((a, b) => {
            if (a[sortField] > b[sortField]) return sortAsc ? 1 : -1;
            if (a[sortField] < b[sortField]) return sortAsc ? -1 : 1;
            return 0;
        });
    }

    const start = (currentPage - 1) * perPage;
    const pageData = data.slice(start, start + perPage);

    const tbody = document.querySelector("#employeeTable tbody");
    tbody.innerHTML = "";

    pageData.forEach((e) => {
        tbody.innerHTML += `
        <tr>
            <td>${e.id}</td>
            <td>${e.name}</td>
            <td>${e.email}</td>
            <td>${e.department}</td>
            <td>${formatRupiah(e.salary)}</td>
            <td>
                <button class="action-btn edit" onclick="editEmployee(${e.id},'${e.name}','${e.email}','${e.department}',${e.salary})">
                    <i class="fa fa-pen"></i>
                </button>
                <button class="action-btn delete" onclick="deleteEmployee(${e.id})">
                    <i class="fa fa-trash"></i>
                </button>
            </td>
        </tr>
        `;
    });

    renderPagination(data.length);
}

function renderPagination(total) {
    const pages = Math.ceil(total / perPage);
    const p = document.getElementById("pagination");
    p.innerHTML = "";

    for (let i = 1; i <= pages; i++) {
        p.innerHTML += `<button onclick="goPage(${i})" ${i === currentPage ? 'style="background:#2563eb;color:white"' : ""
            }>${i}</button>`;
    }
}

function goPage(p) {
    currentPage = p;
    renderTable();
}

function sortTable(field) {
    sortAsc = sortField === field ? !sortAsc : true;
    sortField = field;

    renderTable();
}

function openModal() {
    document.getElementById("modal").style.display = "flex";
    document.getElementById("modalTitle").innerText = "Add Employee";

    document.getElementById("employeeId").value = "";
    clearErrors();

    document.getElementById("name").value = "";
    document.getElementById("email").value = "";
    document.getElementById("department").value = "";
    document.getElementById("salary").value = "";
}

function closeModal() {
    document.getElementById("modal").style.display = "none";
}

window.onclick = function (e) {
    if (e.target.id === "modal") closeModal();
};

function editEmployee(id, name, email, dept, salary) {
    openModal();

    document.getElementById("modalTitle").innerText = "Edit Employee";
    document.getElementById("employeeId").value = id;

    document.getElementById("name").value = name;
    document.getElementById("email").value = email;
    document.getElementById("department").value = dept;
    document.getElementById("salary").value = formatRupiah(salary).replace("Rp ", "").replace(/\./g, "");
}

function clearErrors() {
    document.getElementById("name").classList.remove("error");
    document.getElementById("email").classList.remove("error");
    document.getElementById("department").classList.remove("error");
    document.getElementById("salary").classList.remove("error");
}

function isValidEmail(email) {
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(email);
}

async function saveEmployee() {
    clearErrors();

    const id = document.getElementById("employeeId").value;

    const name = document.getElementById("name").value.trim();
    const email = document.getElementById("email").value.trim();
    const department = document.getElementById("department").value.trim();
    let salaryRaw = document.getElementById("salary").value.replace(/\./g, "").trim();

    if (name.length === 0 || name.length > 200) {
        showError("name", "Name must be 1-200 characters");
        return;
    }

    if (!isValidEmail(email) || email.length > 200) {
        showError("email", "Invalid email or too long (>200 chars)");
        return;
    }

    if (department.length === 0 || department.length > 200) {
        showError("department", "Department must be 1-200 characters");
        return;
    }

    let salaryNumber = parseFloat(salaryRaw);
    if (isNaN(salaryNumber) || salaryNumber < 0) {
        showError("salary", "Salary must be a positive number");
        return;
    }

    if (salaryNumber > 9999999999999999.99) {
        showError("salary", "Salary too large");
        return;
    }

    showLoading(true);

    const data = {
        name,
        email,
        department,
        salary: salaryNumber,
    };

    try {
        if (id) {
            await fetch(API + "/" + id, {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(data),
            });
            toast("Employee updated");
        } else {
            await fetch(API, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(data),
            });
            toast("Employee created");
        }
    } catch {
        toast("Error saving employee", "error");
    }

    closeModal();
    await loadEmployees();
    showLoading(false);
}

function showError(fieldId, msg) {
    const el = document.getElementById(fieldId);
    el.classList.add("error");
    toast(msg, "error");
}

async function deleteEmployee(id) {
    if (!confirm("Delete employee?")) return;

    showLoading(true);

    try {
        await fetch(API + "/" + id, { method: "DELETE" });
        toast("Employee deleted");
        await loadEmployees();
    } catch {
        toast("Error deleting employee", "error");
    }

    showLoading(false);
}

function searchEmployee() {
    const keyword = document.getElementById("search").value.toLowerCase();

    filteredEmployees = employees.filter(
        (e) =>
            e.name.toLowerCase().includes(keyword) ||
            e.email.toLowerCase().includes(keyword) ||
            e.department.toLowerCase().includes(keyword)
    );

    currentPage = 1;
    renderTable();
}

document.getElementById("salary")?.addEventListener("input", function () {
    // Remove non-digits
    let val = this.value.replace(/\D/g, "");

    if (val === "") return (this.value = "");

    // Format with thousand separator
    this.value = Number(val).toLocaleString("id-ID");
});

loadEmployees();