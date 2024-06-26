document.addEventListener('DOMContentLoaded', () => {
    const apiUrl = 'https://localhost:5001/api';
    let jwtToken = '';

    const imageList = document.getElementById('imageList');
    const searchInput = document.getElementById('searchInput');
    const searchButton = document.getElementById('searchButton');
    const pagination = document.getElementById('pagination');
    const usernameInput = document.getElementById('username');
    const passwordInput = document.getElementById('password');
    const loginButton = document.getElementById('loginButton');
    const messageModal = new bootstrap.Modal(document.getElementById('messageModal'), {
        keyboard: false
    });
    const messageModalBody = document.getElementById('messageModalBody');
    const loadingSpinner = document.getElementById('loadingSpinner');
    let currentPage = 1;
    const pageSize = 10;

    loginButton.addEventListener('click', () => {
        login(usernameInput.value, passwordInput.value);
    });

    searchButton.addEventListener('click', () => {
        fetchImages(searchInput.value, currentPage);
    });

    async function login(username, password) {
        try {
            const response = await fetch(`${apiUrl}/account/login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ username, password })
            });

            if (response.ok) {
                const data = await response.json();
                jwtToken = data.token;
                showMessage('Login successful', 'success');
            } else {
                showMessage('Login failed', 'danger');
            }
        } catch (error) {
            console.error('Error logging in:', error);
            showMessage('Error logging in', 'danger');
        }
    }

    async function fetchImages(location, page) {
        try {
            loadingSpinner.style.display = 'block';
            const response = await fetch(`${apiUrl}/image/${location}?page=${page}&pageSize=${pageSize}`, {
                headers: {
                    'Authorization': `Bearer ${jwtToken}`
                }
            });
            if (response.ok) {
                const images = await response.json();
                displayImages(images.data);
                setupPagination(images.totalPages);
            } else {
                showMessage('Error fetching images', 'danger');
            }
        } catch (error) {
            console.error('Error fetching images:', error);
            showMessage('Error fetching images', 'danger');
        } finally {
            loadingSpinner.style.display = 'none';
        }
    }

    function displayImages(images) {
        imageList.innerHTML = '';
        images.forEach(image => {
            const imageItem = document.createElement('div');
            imageItem.classList.add('image-item', 'col-md-3');
            imageItem.innerHTML = `
                <img src="${image.imageUrl}" alt="${image.description}" class="img-fluid">
                <p>${image.description}</p>
            `;
            imageList.appendChild(imageItem);
        });
    }

    function setupPagination(totalPages) {
        pagination.innerHTML = '';
        for (let i = 1; i <= totalPages; i++) {
            const pageButton = document.createElement('button');
            pageButton.classList.add('btn', 'btn-secondary', 'mx-1');
            pageButton.textContent = i;
            pageButton.addEventListener('click', () => {
                currentPage = i;
                fetchImages(searchInput.value, currentPage);
            });
            pagination.appendChild(pageButton);
        }
    }

    function showMessage(message, type) {
        messageModalBody.textContent = message;
        messageModalBody.className = `alert alert-${type}`;
        messageModal.show();
    }
});
