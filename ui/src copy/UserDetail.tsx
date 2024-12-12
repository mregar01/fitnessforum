import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import RenderHTML from './RenderHTML';
import PrintDate from './PrintDate'
import PrintBadges from './PrintBadges'
import profilepic from './images/profilepic.png'
import cakeicon from './images/cakeicon.jpg'
import clockicon from './images/clockicon.png'
import linkicon from './images/linkicon.png'
import geoicon from './images/geoicon.png'

interface User {
  displayName: string;
  creationDate: string;
  lastAccessDate: string;
  websiteUrl: string;
  location: string;
  reputation: number;
  answers: number;
  questions: number;
  aboutMe: string;
  goldBadges: Badge[];
  silverBadges: Badge[];
  bronzeBadges: Badge[];
  posts: UserPost[];
}

interface Badge {
  id: number;
  name: string;
  date: string;
}

interface UserPost {
  id: number;
  postTypeId: number;
  votes: number;
  title?: string;
  parentTitle?: string;
  parentId: number;
  creationDate: string;
}


const UserDetail: React.FC = () => {
  const { userid } = useParams();
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const response = await fetch(`/api/users/${userid}`);
        const userData = await response.json();
  
        setUser(userData);
        setIsLoading(false);
      } catch (error) {
        console.error(error);
      }
    };
  
    fetchUser();
  }, [userid]);
  

  if (isLoading) {
    return <div>Loading user...</div>;
  }

  return (
    <div>
      <Link to="/">
          <button className='btn btn-primary mx-4'>Go to Main Page</button>
      </Link>
      <div className='row my-5'>
        <div className='col-2 offset-1'>
        <img        
          className="MonkeyBig img-fluid"
          src={profilepic}
          alt="monkey"
        />   
        </div>             
        <div className='col'>
          <h3>{user?.displayName}</h3>
          <p className='text-secondary'>
            <img
              className='cake-icon'
              src={cakeicon}
              alt='cake'
            />
            Member since <PrintDate dateString={user?.creationDate} monthType="long" type='user'/>
            <img
              className='clock-icon'
              src={clockicon}
              alt='clock'
            />
            Last seen <PrintDate dateString={user?.lastAccessDate} monthType="long" type='user'/><br></br>
            <img
              className='link-icon'
              src={linkicon}
              alt='link'
            />
            <a href={user?.websiteUrl} target="_blank" rel="noreferrer">{user?.websiteUrl}</a>

            <img
              className='geo-icon'
              src={geoicon}
              alt='geo'
            />
            {user?.location}
          </p>
        </div>
      </div>      
      <div className='row mb-5'>
        <div className='offset-1 col-12 col-sm-2'>
          <h3>Stats</h3>
          <div className='container border border-secondary rounded mb-2'>
            <div className='row'>
              <div className='col-xl-6 col-12 '>
                <strong>{user?.reputation}</strong><br></br>reputation<br></br>
                <strong>{user?.answers}</strong><br></br>answers
              </div>
              <div className='col-xl-6'>
                <strong>X</strong><br></br>reached<br></br>
                <strong>{user?.questions}</strong><br></br>questions
              </div>
            </div>
          </div>          
        </div>
        <div className='col about'>
          <div className='about-body'>
          <h3>About</h3>
            <RenderHTML HTML={user?.aboutMe}/>
          </div>
          <div className='badges'>
            <h3>Badges</h3>
            <div className='row'>
              <PrintBadges badges={user?.goldBadges} section='gold' />
              <PrintBadges badges={user?.silverBadges} section='silver' />
              <PrintBadges badges={user?.bronzeBadges} section='bronze' />
            </div>
          </div>        
          <div className='posts-user'>
            <h3 className='my-3'>Posts</h3>
            <div className='row'>
              <div className='col-11 border border-secondary rounded'>
                {user?.posts && user.posts.length > 0 ? (
                  user.posts.map((userPost) => (
                    <div key={userPost.id}>
                      <div className='row'>
                        <div className='col-1'>
                          {userPost.postTypeId === 1 ? (
                            <p>Q</p>
                          ) : (
                            <p>A</p>
                          )
                          }
                        </div>
                        <div className='col-1'>
                          {userPost.votes}
                        </div>
                        <div className='col-8'>
                          {userPost.title ? (
                            <Link to={`/posts/${userPost.id}`}>{userPost.title}</Link>
                          ) : (
                            <Link to={`/posts/${userPost.parentId}`}>{userPost.parentTitle}</Link>
                          )
                          }
                        </div>
                        <div className='col-2'>
                          <PrintDate dateString={userPost.creationDate} monthType="short" type='user'/>
                        </div>
                      </div>
                      <hr></hr>
                    </div>
                  ))
                ) : (
                  <p>No posts available.</p>
                )}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default UserDetail;
