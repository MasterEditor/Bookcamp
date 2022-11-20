import React from "react";
import { Link } from "react-router-dom";
import "../forbiddent.scss";

function Forbidden() {
  return (
    <div className="forBody">
      <div id="app">
        <div>403</div>
        <div className="txt">
          Forbidden<span className="blink">_</span>
        </div>
        <Link to="/" className="text-sm mt-5 cursor-pointer">
          Back
        </Link>
      </div>
    </div>
  );
}

export default Forbidden;
