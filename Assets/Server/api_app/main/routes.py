from flask import Blueprint,request,jsonify, redirect, url_for
from api_app.models import User, Game
from dotenv import load_dotenv
import os
load_dotenv()

from api_app import app, db, bcyrpt

main = Blueprint('main',__name__)



@main.route('/')
def home_response():
    response = {
        "Response":"GOOD",
        "Data":None
    }
    
    return jsonify(response)

@main.route('/API/Create/User',methods=['POST'])
def create_user():
    response_key = request.form.get('KEY')
    response_name = request.form.get('NAME')
    response_password = request.form.get("PASSWORD")
    if response_name and bcyrpt.check_password_hash(bcyrpt.generate_password_hash(os.getenv("SECRET_PASSCODE")),
        response_key):
        
        new_user = User(name=response_name,
            password=bcyrpt.generate_password_hash(response_password))
        db.session.add(new_user)
        db.session.commit()
        print(User.query.get(new_user.id))
        response = {
            "Response":"SUCCESS",
            "Data":[{'USER_ID':new_user.id},{"USER_PASSWORD":response_password}]
        }
        return jsonify(response)
    response = {
        "Response":"ERROR",
        "Data":["ERROR PROCESSING REQUEST CHECK KEY AND REQUIRED PARAMETERS"]
    }
    return jsonify(response)