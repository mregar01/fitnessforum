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

interface Response {
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
  comments: Comment[];
  parentId: number;
}

interface EditResponseProps {
  RESPONSE?: Response;
}

const EditResponse: React.FC<EditResponseProps> = ({ RESPONSE }) => {
  if (!RESPONSE) {
    return null;
  }

  const [showPopup, setShowPopup] = useState(false);
  const [newBody, setNewBody] = useState('');

  const openPopup = () => {
    setNewBody(RESPONSE?.body || '');
    setShowPopup(true);
  };

  const closePopup = () => {
    setShowPopup(false);
  };

  const handleBodyChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
    setNewBody(e.target.value);
  };

  const handleSubmit = async () => {
    const responseId = RESPONSE?.id;
    const apiUrl = `/api/answers/edit/${responseId}`;
    const requestBody = {
      parentId: RESPONSE.parentId,
      responseBody: newBody,
    };

    console.log(responseId);
    console.log(newBody);

    try {
      const response = await fetch(apiUrl, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(requestBody),
      });

      if (response.ok) {
        window.location.href = `/posts/${RESPONSE.parentId}`;
      } else {
        console.error('Error editing response');
      }
    } catch (error) {
      console.error(error);
    }

    closePopup();
  };

  return (
    <div>
      <button className="btn btn-primary" onClick={openPopup}>
        Edit Response
      </button>

      {showPopup && (
        <div className="popup-overlay">
          <div className="popup">
            <h2>Edit Response</h2>
            <div className="form-group">
              <label htmlFor="body">Body:</label>
              <textarea
                id="body"
                className="form-control"
                value={newBody}
                onChange={handleBodyChange}
                rows={8}
              />
            </div>
            <div className="button-group">
              <button
                className="btn btn-primary mx-2 my-2"
                onClick={handleSubmit}
              >
                Submit
              </button>
              <button className="btn btn-secondary my-2" onClick={closePopup}>
                Cancel
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default EditResponse;