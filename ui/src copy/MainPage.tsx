import React, { useState, useEffect, useCallback } from 'react';
import { Link } from 'react-router-dom';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import Dropdown from 'react-bootstrap/Dropdown';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import Cookies from 'js-cookie';
import PostList from './PostList'


interface Post {
  id: number;
  votes: number;
  answerCount: number;
  viewCount: number;
  title: string;
  tags: string;
  parentId: number;
  parentTitle: string;
  parentTags: string;
  acceptedAnswerId: number;
}


interface User {
  id: number;
  displayName: string;
}

interface CurrUser {
  userId: number;
  displayName: string;
}


const MainPage: React.FC = () => {
  const [posts, setPosts] = useState<Post[]>([]);
  const [currentPage, setCurrentPage] = useState<number>(1);
  const pageSize: number = 25;
  const [users, setUsers] = useState<User[]>([]);
  const [selectedUser, setSelectedUser] = useState<CurrUser | undefined>();
  const [showModal, setShowModal] = useState<boolean>(true);
  const [profileSelected, setProfileSelected] = useState<boolean>(false);
  const [searchQuery, setSearchQuery] = useState<string>('');
  const [useOpenSearch, setUseOpenSearch] = useState<boolean>(false);


  const userId = Cookies.get('Id');
  const userName = Cookies.get('Name');

  const handleSearch = () => {
    if (searchQuery.trim() !== '') {
      const encodedQuery = encodeURIComponent(searchQuery.trim());
      window.location.href = `/search/${encodedQuery}/${useOpenSearch}`;
    }
  };


  const selectUser = useCallback(async () => {
    if (userId && userName) {
      try {
        const response = await fetch(`/api/session/user`, {
          method: 'GET',
          credentials: 'include',
          mode: "cors"
        });
        if (response.ok) {
          const data = await response.json();
          setSelectedUser(data);
        } else {
          throw new Error(`HTTP status ${response.status}`);
        }
      } catch (error) {
        console.error("Error getting current user", error);
      }
    } else {
      return null;
    }
  }, [userId, userName]);



  useEffect(() => {
    fetchTopUsers();
    if (userId && userName) {
      setProfileSelected(true);
    }
    if (!selectedUser) {
      selectUser();
    }
  }, [selectedUser, userId, userName, selectUser]);

  const fetchTopUsers = async () => {
    try {
      const response = await fetch('/api/users/top', {
        method: 'GET',
        credentials: 'include',
      });

      if (response.ok) {
        const data = await response.json();
        setUsers(data);
      } else {
        throw new Error(`HTTP status ${response.status}`);
      }
    } catch (error) {
      console.error('Error fetching top users:', error);
    }
  };

  const renderProfileSelectionModal = () => {
    if (!profileSelected) {
      return (
        <Modal show={showModal} backdrop="static" keyboard={false}>
          <Modal.Header>
            <Modal.Title>Select a Profile To Continue</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            {/* Render the user selection dropdown here */}
            <Dropdown onSelect={(eventKey) => handleUserSelect(users.find(user => user.id === Number(eventKey)))}>
              <Dropdown.Toggle variant="primary" id="user-dropdown">
                Select a user
              </Dropdown.Toggle>
              <Dropdown.Menu>
                {users.map((user) => (
                  <Dropdown.Item key={user.id} eventKey={user.id.toString()}>
                    {user.displayName}
                  </Dropdown.Item>
                ))}
              </Dropdown.Menu>
            </Dropdown>
          </Modal.Body>
          <Modal.Footer>
            <Button variant="primary" disabled={!selectedUser} onClick={() => setShowModal(false)}>
              Continue
            </Button>
          </Modal.Footer>
        </Modal>
      );
    }
    return null;
  };

  useEffect(() => {
    return () => {
      setProfileSelected(false); // Reset the profileSelected state
    };
  }, []);

  const handleUserSelect = async (user: User | undefined) => {
    await setCurrentUser(user); // Set the current user on the backend
    selectUser();
    setShowModal(false);
    setProfileSelected(true);
  };

  const setCurrentUser = async (user: User | undefined) => {
    try {
      const response = await fetch('/api/session/user', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          userId: user?.id,
          displayName: user?.displayName,
        }),
        credentials: 'include',
        mode: 'cors',
      });

      if (response.ok) {
      } else {
        throw new Error(`HTTP status ${response.status}`);
      }
    } catch (error) {
      console.error('Error setting current user:', error);
    }
  };

  const fetchPosts = useCallback(async () => {
    try {
      const response = await fetch(`/api/posts?page=${currentPage}&pageSize=${pageSize}`, {
        method: 'GET',
        credentials: 'include',
      });

      if (response.ok) {
        const data = await response.json();
        setPosts(data);
      } else {
        throw new Error(`HTTP status ${response.status}`);
      }
    } catch (error) {
      console.error(error);
    }
  }, [currentPage]);


  useEffect(() => {
    fetchPosts();
  }, [fetchPosts]);

  useEffect(() => {
    setCurrentPage(1);
  }, []);

  const goToNextPage = () => {
    setCurrentPage((prevPage) => prevPage + 1);
    window.scrollTo(0, 0);
  };

  const goToPreviousPage = () => {
    setCurrentPage((prevPage) => prevPage - 1);
    window.scrollTo(0, 0);
  };

  const goToFirstPage = () => {
    setCurrentPage(1);
    window.scrollTo(0, 0);
  };

  return (
    <div>
      {renderProfileSelectionModal()}
      <div className='container bg-secondary sticky-top my-3 p-4'>
        <div className='row justify-content-center'>
          <div className='col-md-12 col-xl-6'>
            <h1>
              Fitness MaxExchange Page {currentPage}
            </h1>
          </div>
          <div className='col-xl-2 col-md-4 d-flex align-items-center justify-content-center my-2'>
            <Link to="/posts/new">
              <button className='btn btn-primary'>Ask a question</button>
            </Link>
          </div>
          <div className='col-xl-1 col-md-4 d-flex align-items-center justify-content-center my-2'>
            <Dropdown onSelect={(eventKey) => handleUserSelect(users.find(user => user.id === Number(eventKey)))}>
              <Dropdown.Toggle variant="primary" id="user-dropdown">
                Select a user
              </Dropdown.Toggle>
              <Dropdown.Menu>
                {users.map((user) => (
                  <Dropdown.Item key={user.id} eventKey={user.id.toString()}>
                    {user.displayName}
                  </Dropdown.Item>
                ))}
              </Dropdown.Menu>
            </Dropdown>
          </div>
          <div className='col d-flex align-items-center justify-content-center my-2'>
            <Link to={`/users/${selectedUser?.userId}`} className='display-name'>
              <button className='btn btn-primary'>{selectedUser?.displayName}'s Profile</button>
            </Link>
          </div>
        </div>
      </div>      
      <div className='container d-flex align-items-center justify-content-center my-2'>
        <input
          type='text'
          className='form-control'
          value={searchQuery}
          onChange={(e) => setSearchQuery(e.target.value)}
          placeholder='Search...'
        />
        <button className='btn btn-primary mx-2' onClick={handleSearch}>
          Search
        </button>
        <button
          className={`btn ${useOpenSearch ? 'btn-success' : 'btn-secondary'} mx-2`}
          onClick={() => setUseOpenSearch(!useOpenSearch)}
        >
          {useOpenSearch ? 'Open Search On' : 'Open Search Off'}
        </button>
      </div>
      <PostList posts={posts} type="normal"/>

      <div className="container justify-content-center m-5 sticky-bottom row">
        <button
          className="btn btn-primary col-3 m-2"
          disabled={currentPage === 1}
          onClick={goToFirstPage}
        >
          First
        </button>
        <button
          className="btn btn-primary col-3 m-2"
          disabled={currentPage === 1}
          onClick={goToPreviousPage}
        >
          Previous
        </button>
        <button
          className="btn btn-primary col-3 m-2"
          disabled={posts.length < pageSize}
          onClick={goToNextPage}
        >
          Next
        </button>
      </div>
    </div>
  );
};

export default MainPage;
