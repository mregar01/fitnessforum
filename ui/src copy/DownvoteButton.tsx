import React from 'react';
import './App.css'

interface DownvoteButtonProps {
  postId: number | undefined;
  onDownvote: () => void; // Callback function to update the post after downvoting
}

const DownvoteButton: React.FC<DownvoteButtonProps> = ({ postId, onDownvote }) => {

  const handleDownvote = () => {
    if (postId !== undefined) {
      fetch(`/api/posts/${postId}/decrement`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
      })
        .then(response => {
          if (response.ok) {
            onDownvote(); // Call the callback function to update the post
          } else {
            console.error('Failed to downvote the post');
          }
        })
        .catch(error => {
          console.error('Error occurred while downvoting:', error);
        });
    } else {
      console.error('Invalid postId');
    }
  };

  return (
    <button className='downvote-button my-2' onClick={handleDownvote}>
    </button>
  );
};

export default DownvoteButton;



