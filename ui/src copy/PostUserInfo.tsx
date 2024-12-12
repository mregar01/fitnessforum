import React from "react";
import { Link } from "react-router-dom";
import PrintDate from './PrintDate'

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

interface Post {
  postItem: {
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
  };
  responses: Response[];
}

interface Comment {
  id: number;
  score: number;
  text: string;
  userDisplayName: string;
  userId: number;
  creationDate: string;
}

interface PostUserInfoProps {
  item: Response | Post | null ;
}

const PostUserInfo: React.FC<PostUserInfoProps> = ({ item }) => {
  if (!item) {
    return null; // Render null when item is null
  }
  
  const isPost = "responses" in item; // Check if it's a Post item

  const { id, title, votes, body, creationDate, ownerUserId, ownerDisplayName, ownerRep, ownerGoldBadges, ownerSilverBadges, ownerBronzeBadges, comments } = isPost ? (item as Post).postItem : (item as Response);

  const prefix = isPost ? <small>asked</small> : <small>answered</small>;


  return (
    <>
      {prefix}{" "}
      <PrintDate dateString={creationDate} type='post'/>
      <div>
        <div className="row">
          <div className="col-md-2 m-1">
            <img
              className="Monkey"
              src="https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png"
              alt="monkey"
            />
          </div>
          <div className="col-md-9">
            <Link
              to={`/users/${ownerUserId}`}
              className="display-name"
            >
              {ownerDisplayName}
            </Link>{" "}
            <br />
            <div className="reputation">
              {ownerRep}
              {/* Badges */}
              <span className="gold-dot"></span>
              {ownerGoldBadges}
              <span className="silver-dot"></span>
              {ownerSilverBadges}
              <span className="bronze-dot"></span>
              {ownerBronzeBadges}
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default PostUserInfo;
