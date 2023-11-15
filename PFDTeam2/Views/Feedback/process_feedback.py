import spacy

def process_feedback(feedback_text):
    nlp = spacy.load("en_core_web_sm")
    doc = nlp(feedback_text)

    # Example 1: Print tokens and part-of-speech tags
    for token in doc print(f"{token.text}: {token.pos_}")
    
    # Example 2: Extract named entities
    entities = [ent.text for ent in doc.ents]
    print("Named Entities:", entities)

    # Additional processing logic can be added based on requirements
    relvant_keywords = ["onboarding", "employee", "improvement"]
    is_relevant = any(keyword in feedback_text.lower() for keyword in relevant_keywords)

    return is_relevant