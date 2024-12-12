import React from "react";
import { Link } from "react-router-dom";
import PrintDate from "./PrintDate";

interface Comment {
  id: number;
  score: number;
  text: string;
  userId: number;
  userDisplayName: string;
  creationDate: string;
}

interface PrintCommentsProps {
  comments: Comment[] | undefined;
}

const PrintComments: React.FC<PrintCommentsProps> = ({ comments }) => {
  if (!comments || comments.length === 0) {
    return <p><br /></p>;
  }

  return (
    <>
      {comments.map((comment) => (
        <div className="row" key={comment.id}>
          <hr />
          <small className="col-1 text-center">{comment.score}</small>
          <small className="col-11">
            {comment.text} -{" "}
            <small className="text-primary">
              <Link to={`/users/${comment.userId}`}>
                {comment.userDisplayName}
              </Link>
              <PrintDate dateString={comment.creationDate} className="text-muted" type='post'/>
            </small>
            <br />
          </small>
        </div>
      ))}
    </>
  );
};

export default PrintComments;
