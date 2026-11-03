import streamlit as st
from matem import Matem

st.markdown("""
<style>

    div.stButton > button {
        font-size: 30px !important; 
        color: #ffb6c1 !important; /* Нежно-розовый текст */
        border-radius: 70px !important;
        /* остальные стили кнопки, если нужны */
    }
    /* Эффект при наведении (чуть ярче) */
    div.stButton > button:hover {
        color: #ff69b4 !important; 
    }
</style>
""", unsafe_allow_html=True)
 
if "msg" not in st.session_state:
    st.session_state.msg = "|"
    st.session_state.mat = Matem()
 
def print(message):
    st.session_state.msg = message
    st.rerun()

st.markdown(
    f"<p style='font-size: 48px; text-align: right;'>{st.session_state.msg}</p>", 
    unsafe_allow_html=True
)
c1, c2, c3, c4, c5, c6, cl = st.columns(7)

def process_click(label):
    result = st.session_state.mat.calculate(label)
    print(result)

with cl: 
    # Передаем "delete" вместо "D"
    if st.button("⌫", use_container_width=True): 
        process_click("delete")

st.divider()

col1, col2, col3, col4, col5, col6, col7 = st.columns(7)

with col1:
    for label in ["sin", "ln", "e^x", "|x|"]:
        if st.button(label, use_container_width=True): process_click(label)

with col2:
    for label in ["cos", "log", "x^2", "p"]:
        if st.button(label, use_container_width=True): process_click(label)

with col3:
    for label in ["sqrt", "tan", "1/x", "x^y", "e"]:
        if st.button(label, use_container_width=True): process_click(label)

with col4:
    for label in ["C", "7", "4", "1", "+/-"]:
        if st.button(label, use_container_width=True): process_click(label)


with col5:
    for label in ["()", "8", "5", "2", "0"]:
        if st.button(label, use_container_width=True): process_click(label)

with col6:
    for label in ["%", "9", "6", "3", ","]:
        if st.button(label, use_container_width=True): process_click(label)

with col7:
    for label in ["/", "x", "-", "+", "="]:
        if st.button(label, use_container_width=True): process_click(label)

