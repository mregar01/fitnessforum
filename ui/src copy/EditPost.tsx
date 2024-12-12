import React, { useState } from 'react';
import './App.css';

interface Comment {
  id: number;
  score: number;
  text: string;
  userDisplayName: string;
  userId: number;
  creationDate: string;
}

interface Post {
  id: number;
  title: string;
  votes: number;
  body: string;
  creationDate: string;
  ownerUserId: number;
  ownerDisplayName: string;
  ownerRep: number;
  ownerGoldBadges: number;
  ownerSilverBadges: number;
  ownerBronzeBadges: number;
  tags: string;
  comments: Comment[];
}

interface EditPostProps {
  POST?: Post;
}

const EditPost: React.FC<EditPostProps> = ({ POST }) => {
  const [showPopup, setShowPopup] = useState(false);
  const [newTitle, setNewTitle] = useState('');
  const [newBody, setNewBody] = useState('');
  const [newTags, setNewTags] = useState('');

  const openPopup = () => {
    setNewTitle(POST?.title || '');
    setNewBody(POST?.body || '');
    setNewTags(POST?.tags || '');
    setShowPopup(true);
  };

  const closePopup = () => {
    setShowPopup(false);
  };

  const handleTitleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setNewTitle(e.target.value);
  };

  const handleBodyChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
    setNewBody(e.target.value);
  };

  const handleTagsChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setNewTags(e.target.value);
  };

  const handleSubmit = async () => {
    const postId = POST?.id;
    const apiUrl = `/api/posts/edit/${postId}`;
    const requestBody = {
      postTitle: newTitle,
      postBody: newBody,
      postTags: newTags
    };

    try {
      const response = await fetch(apiUrl, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(requestBody)
      });

      if (response.ok) {
        window.location.href = `/posts/${postId}`;
      } else {
        console.error("error editing post");
      }
    } catch (error) {
      console.error(error);
    }

    closePopup();
  };

  if (!POST) {
    return null;
  }

  return (
    <div>
      <button className="btn btn-primary" onClick={openPopup}>Edit Post</button>

      {showPopup && (
        <div className="popup-overlay">
          <div className="popup">
            <h2>Edit Post</h2>
            <div className="form-group">
              <label htmlFor="title">Title:</label>
              <input type="text" id="title" className="form-control" value={newTitle} onChange={handleTitleChange}/>
            </div>
            <div className="form-group">
              <label htmlFor="body">Body:</label>
              <textarea id="body" className="form-control" value={newBody} onChange={handleBodyChange} rows={8}/>
            </div>
            <div className="form-group">
              <label htmlFor="tags">Tags:</label>
              <input type="text" id="tags" className="form-control" value={newTags} onChange={handleTagsChange} />
            </div>
            <div className="button-group">
              <button className="btn btn-primary mx-2 my-2" onClick={handleSubmit}>Submit</button>
              <button className="btn btn-secondary my-2" onClick={closePopup}>Cancel</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default EditPost;
